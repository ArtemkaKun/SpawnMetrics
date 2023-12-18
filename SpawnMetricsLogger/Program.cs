using System.CommandLine;
using System.Text;
using System.Text.Json;
using LibGit2Sharp;
using SharedConstants;
using SpawnMetricsLogger;
using SpawnMetricsLogger.Config;
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
    var config = ConfigManager.TryReadConfigFromFile(configPath);

    if (config == null)
    {
        return;
    }

    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(config.DataServerUrl);
    httpClient.DefaultRequestHeaders.Add(MetricsControllerConstants.ApiKeyParameter, adminApiKey);

    if (isLogEveryCommit)
    {
        using var repo = new Repository(repoPath);

        Commands.Checkout(repo, config.BranchName);

        var remote = repo.Network.Remotes[config.RemoteName];
        var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(repo, config.RemoteName, refSpecs, null, "Ref was updated");

        foreach (var commit in repo.Branches[config.BranchName].Commits)
        {
            Commands.Checkout(repo, commit, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force, CheckoutNotifyFlags = CheckoutNotifyFlags.None });

            var metrics = MetricsCollector.CollectMetrics(commit, repoPath, config);

            if (metrics == null)
            {
                return;
            }

            foreach (var metric in metrics)
            {
                var requestBody = new LogMetricRequestBody
                {
                    ProjectName = config.ProjectName,
                    Metric = metric
                };

                var requestBodyJson = JsonSerializer.Serialize(requestBody);

                var response = httpClient.PutAsync(EndpointsConstants.MetricEndpoint, new StringContent(requestBodyJson, Encoding.UTF8, "application/json")).Result;

                if (response.IsSuccessStatusCode == false)
                {
                    Console.WriteLine($"Failed to log metric {metric.Name}");
                }
                else
                {
                    Console.WriteLine($"Logged metric {commit.Sha}");
                }
            }
        }

        Commands.Checkout(repo, config.BranchName);
    }
    else
    {
        using var repo = new Repository(repoPath);

        Commands.Checkout(repo, config.BranchName);

        var currentCommit = repo.Head.Tip;

        var metrics = MetricsCollector.CollectMetrics(currentCommit, repoPath, config);

        if (metrics == null)
        {
            return;
        }

        foreach (var metric in metrics)
        {
            var requestBody = new LogMetricRequestBody
            {
                ProjectName = config.ProjectName,
                Metric = metric
            };

            var requestBodyJson = JsonSerializer.Serialize(requestBody);

            var response = httpClient.PutAsync(EndpointsConstants.MetricEndpoint, new StringContent(requestBodyJson, Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode == false)
            {
                Console.WriteLine($"Failed to log metric {metric.Name}");
            }
            else
            {
                Console.WriteLine($"Logged metric {currentCommit.Sha}");
            }
        }
    }
}, repoPathOption, configPathOption, adminApiKeyOption, logEveryCommitOption);

return rootCommand.Invoke(args);