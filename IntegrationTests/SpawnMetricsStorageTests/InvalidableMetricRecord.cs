namespace IntegrationTests.SpawnMetricsStorageTests;

public readonly struct InvalidableMetricRecord
{
    public readonly string? Name;
    public readonly DateTime? LogTimeUtc;
    public readonly string? CommitGitHubUrl;
    public readonly string? CommitMessage;
    public readonly string? Value;
    public readonly string? Units;

    public InvalidableMetricRecord(string? name, DateTime? logTimeUtc, string? commitGitHubUrl, string? commitMessage, string? value, string? units)
    {
        Name = name;
        LogTimeUtc = logTimeUtc;
        CommitGitHubUrl = commitGitHubUrl;
        CommitMessage = commitMessage;
        Value = value;
        Units = units;
    }
}