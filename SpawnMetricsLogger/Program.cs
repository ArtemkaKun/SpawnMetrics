using System.CommandLine;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using LibGit2Sharp;
using MetricRecordModel;
using MiniValidation;
using SharedConstants;
using SpawnMetricsLogger;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;

var repoPathOption = new Option<string>("--repo-path", description: "The path to the repository");
var configPathOption = new Option<string>("--config-path", description: "The path to the config file");
var adminApiKeyOption = new Option<string>("--admin-api-key", description: "The admin API key");
var logEveryCommitOption = new Option<bool>("--log-every-commit", getDefaultValue: () => false, description: "Log every commit if set");

var rootCommand = new RootCommand
{
    repoPathOption,
    configPathOption,
    adminApiKeyOption,
    logEveryCommitOption
};

rootCommand.SetHandler((repoPath, configPath, adminApiKey, isLogEveryCommit) =>
{
    var config = JsonSerializer.Deserialize<ConfigModel>(File.ReadAllText(configPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (config == null)
    {
        Console.WriteLine("Failed to deserialize config");
        return;
    }

    var isValid = MiniValidator.TryValidate(config, out var errors);

    if (!isValid)
    {
        Console.WriteLine("Config is invalid:");

        foreach (var error in errors)
        {
            Console.WriteLine(error.Key);

            foreach (var errorMessage in error.Value)
            {
                Console.WriteLine(errorMessage);
            }
        }

        return;
    }

    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(config.dataServerUrl);
    httpClient.DefaultRequestHeaders.Add(MetricsControllerConstants.ApiKeyParameter, adminApiKey);

    if (isLogEveryCommit)
    {
        using var repo = new Repository(repoPath);

        Commands.Checkout(repo, config.branchName);

        var remote = repo.Network.Remotes[config.remoteName];
        var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(repo, config.remoteName, refSpecs, null, "Ref was updated");

        foreach (var commit in repo.Branches[config.branchName].Commits)
        {
            Commands.Checkout(repo, commit, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force, CheckoutNotifyFlags = CheckoutNotifyFlags.None });

            var commitLogTimeUtc = commit.Committer.When.UtcDateTime;
            var commitGitHubUrl = $"{config.baseCommitGitHubUrl}{commit.Sha[..MetricRecordConstants.ShortCommitHashLength]}";

            foreach (var metricOperation in config.metricOperations)
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bash",
                    WorkingDirectory = repoPath,
                    Arguments = $"-c \"{metricOperation.command}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process == null)
                {
                    Console.WriteLine($"Failed to start process for metric {metricOperation.name}");
                    return;
                }

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"Failed to get metric {metricOperation.name}");
                    return;
                }

                var output = process.StandardOutput.ReadToEnd();

                if (string.IsNullOrWhiteSpace(output))
                {
                    Console.WriteLine($"Failed to get metric {metricOperation.name}");
                    return;
                }

                var metric = new MetricRecord(metricOperation.name, commitLogTimeUtc, commitGitHubUrl, commit.Message.Trim(), output.Trim(), metricOperation.units);

                var requestBody = new LogMetricRequestBody
                {
                    ProjectName = config.projectName,
                    Metric = metric
                };

                var requestBodyJson = JsonSerializer.Serialize(requestBody);

                var response = httpClient.PutAsync(EndpointsConstants.MetricEndpoint, new StringContent(requestBodyJson, Encoding.UTF8, "application/json")).Result;

                if (response.IsSuccessStatusCode == false)
                {
                    Console.WriteLine($"Failed to log metric {metricOperation.name}");
                }
                else
                {
                    Console.WriteLine($"Logged metric {commit.Sha}");
                }
            }
        }

        Commands.Checkout(repo, config.branchName);
    }
    else
    {
        using var repo = new Repository(repoPath);

        Commands.Checkout(repo, config.branchName);

        var currentCommit = repo.Head.Tip;

        var commitLogTimeUtc = currentCommit.Committer.When.UtcDateTime;
        var commitGitHubUrl = $"{config.baseCommitGitHubUrl}{currentCommit.Sha[..MetricRecordConstants.ShortCommitHashLength]}";

        foreach (var metricOperation in config.metricOperations)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "bash",
                WorkingDirectory = repoPath,
                Arguments = $"-c \"{metricOperation.command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process == null)
            {
                Console.WriteLine($"Failed to start process for metric {metricOperation.name}");
                return;
            }

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Failed to get metric {metricOperation.name}");
                return;
            }

            var output = process.StandardOutput.ReadToEnd();

            if (string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine($"Failed to get metric {metricOperation.name}");
                return;
            }

            var metric = new MetricRecord(metricOperation.name, commitLogTimeUtc, commitGitHubUrl, currentCommit.Message.Trim(), output.Trim(), metricOperation.units);

            var requestBody = new LogMetricRequestBody
            {
                ProjectName = config.projectName,
                Metric = metric
            };

            var requestBodyJson = JsonSerializer.Serialize(requestBody);

            var response = httpClient.PutAsync(EndpointsConstants.MetricEndpoint, new StringContent(requestBodyJson, Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode == false)
            {
                Console.WriteLine($"Failed to log metric {metricOperation.name}");
            }
            else
            {
                Console.WriteLine($"Logged metric {currentCommit.Sha}");
            }
        }
    }
}, repoPathOption, configPathOption, adminApiKeyOption, logEveryCommitOption);

return rootCommand.Invoke(args);