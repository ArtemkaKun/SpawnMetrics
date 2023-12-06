namespace IntegrationTests.SpawnMetricsStorageTests;

public sealed class InvalidableMetricRecord
{
    public string? Name { get; init; }
    public DateTime? LogTimeUtc { get; init; }
    public string? CommitGitHubUrl { get; init; }
    public string? CommitMessage { get; init; }
    public string? Value { get; init; }
    public string? Units { get; init; }
}