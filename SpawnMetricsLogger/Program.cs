using System.CommandLine;
using LibGit2Sharp;
using SpawnMetricsLogger;
using SpawnMetricsLogger.Config;

var repoPathOption = new Option<string>("--repo-path", description: "The path to the repository");
var configPathOption = new Option<string>("--config-path", description: "The path to the config file");
var adminApiKeyOption = new Option<string>("--admin-api-key", description: "The admin API key");
var logOnlyLatestCommitOption = new Option<bool>("--log-only-latest-commit", getDefaultValue: () => false, description: "Log only the latest commit if set, otherwise log every commit in the branch");

var rootCommand = new RootCommand
{
    repoPathOption,
    configPathOption,
    adminApiKeyOption,
    logOnlyLatestCommitOption
};

rootCommand.SetHandler((repoPath, configPath, adminApiKey, isLogOnlyLatestCommit) =>
{
    var config = ConfigManager.TryReadConfigFromFile(configPath);

    if (config == null)
    {
        return;
    }

    var metricsLogger = new MetricsLogger(config, adminApiKey);

    using var repo = new Repository(repoPath);
    Commands.Checkout(repo, config.BranchName);

    if (isLogOnlyLatestCommit)
    {
        var currentCommit = repo.Head.Tip;
        metricsLogger.LogMetrics(currentCommit, repoPath);
    }
    else
    {
        var remote = repo.Network.Remotes[config.RemoteName];
        var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
        Commands.Fetch(repo, config.RemoteName, refSpecs, null, "Ref was updated");

        foreach (var commit in repo.Branches[config.BranchName].Commits)
        {
            Commands.Checkout(repo, commit, new CheckoutOptions { CheckoutModifiers = CheckoutModifiers.Force, CheckoutNotifyFlags = CheckoutNotifyFlags.None });
            metricsLogger.LogMetrics(commit, repoPath);
        }

        Commands.Checkout(repo, config.BranchName);
    }
}, repoPathOption, configPathOption, adminApiKeyOption, logOnlyLatestCommitOption);

return rootCommand.Invoke(args);