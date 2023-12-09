namespace IntegrationTests.SpawnMetricsStorageTests.LogMetricEndpointTests;

public sealed class InvalidableMetricRecordBuilder
{
    private InvalidableMetricRecord _record = new(SpawnMetricsStorageTestsConstants.TestMetricName, DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

    public InvalidableMetricRecordBuilder WithName(string? name)
    {
        _record = new InvalidableMetricRecord(name, _record.LogTimeUtc, _record.CommitGitHubUrl, _record.CommitMessage, _record.Value, _record.Units);

        return this;
    }

    public InvalidableMetricRecordBuilder WithLogTimeUtc(DateTime? logTimeUtc)
    {
        _record = new InvalidableMetricRecord(_record.Name, logTimeUtc, _record.CommitGitHubUrl, _record.CommitMessage, _record.Value, _record.Units);

        return this;
    }

    public InvalidableMetricRecordBuilder WithCommitGitHubUrl(string? commitGitHubUrl)
    {
        _record = new InvalidableMetricRecord(_record.Name, _record.LogTimeUtc, commitGitHubUrl, _record.CommitMessage, _record.Value, _record.Units);

        return this;
    }

    public InvalidableMetricRecordBuilder WithCommitMessage(string? commitMessage)
    {
        _record = new InvalidableMetricRecord(_record.Name, _record.LogTimeUtc, _record.CommitGitHubUrl, commitMessage, _record.Value, _record.Units);

        return this;
    }

    public InvalidableMetricRecordBuilder WithValue(string? value)
    {
        _record = new InvalidableMetricRecord(_record.Name, _record.LogTimeUtc, _record.CommitGitHubUrl, _record.CommitMessage, value, _record.Units);

        return this;
    }

    public InvalidableMetricRecordBuilder WithUnits(string? units)
    {
        _record = new InvalidableMetricRecord(_record.Name, _record.LogTimeUtc, _record.CommitGitHubUrl, _record.CommitMessage, _record.Value, units);

        return this;
    }

    public InvalidableMetricRecord Build()
    {
        return _record;
    }
}