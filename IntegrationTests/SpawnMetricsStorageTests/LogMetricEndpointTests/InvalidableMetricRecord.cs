namespace IntegrationTests.SpawnMetricsStorageTests.LogMetricEndpointTests;

public readonly struct InvalidableMetricRecord(string? name, DateTime? logTimeUtc, string? commitGitHubUrl, string? commitMessage, string? value, string? units)
{
    public readonly string? Name = name;
    public readonly DateTime? LogTimeUtc = logTimeUtc;
    public readonly string? CommitGitHubUrl = commitGitHubUrl;
    public readonly string? CommitMessage = commitMessage;
    public readonly string? Value = value;
    public readonly string? Units = units;
}