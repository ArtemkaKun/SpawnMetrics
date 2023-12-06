namespace IntegrationTests.SpawnMetricsStorageTests;

public sealed class InvalidableMetricRecordBuilder
{
    private InvalidableMetricRecord _record = new()
    {
        Name = "TEST",
        LogTimeUtc = DateTime.UtcNow,
        CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
        CommitMessage = "TEST",
        Value = "TEST",
        Units = "TEST"
    };

    public InvalidableMetricRecordBuilder WithName(string? name)
    {
        _record = new InvalidableMetricRecord
        {
            Name = name,
            LogTimeUtc = _record.LogTimeUtc,
            CommitGitHubUrl = _record.CommitGitHubUrl,
            CommitMessage = _record.CommitMessage,
            Value = _record.Value,
            Units = _record.Units
        };

        return this;
    }

    public InvalidableMetricRecordBuilder WithLogTimeUtc(DateTime? logTimeUtc)
    {
        _record = new InvalidableMetricRecord
        {
            Name = _record.Name,
            LogTimeUtc = logTimeUtc,
            CommitGitHubUrl = _record.CommitGitHubUrl,
            CommitMessage = _record.CommitMessage,
            Value = _record.Value,
            Units = _record.Units
        };

        return this;
    }

    public InvalidableMetricRecordBuilder WithCommitGitHubUrl(string? commitGitHubUrl)
    {
        _record = new InvalidableMetricRecord
        {
            Name = _record.Name,
            LogTimeUtc = _record.LogTimeUtc,
            CommitGitHubUrl = commitGitHubUrl,
            CommitMessage = _record.CommitMessage,
            Value = _record.Value,
            Units = _record.Units
        };

        return this;
    }

    public InvalidableMetricRecordBuilder WithCommitMessage(string? commitMessage)
    {
        _record = new InvalidableMetricRecord
        {
            Name = _record.Name,
            LogTimeUtc = _record.LogTimeUtc,
            CommitGitHubUrl = _record.CommitGitHubUrl,
            CommitMessage = commitMessage,
            Value = _record.Value,
            Units = _record.Units
        };

        return this;
    }

    public InvalidableMetricRecordBuilder WithValue(string? value)
    {
        _record = new InvalidableMetricRecord
        {
            Name = _record.Name,
            LogTimeUtc = _record.LogTimeUtc,
            CommitGitHubUrl = _record.CommitGitHubUrl,
            CommitMessage = _record.CommitMessage,
            Value = value,
            Units = _record.Units
        };

        return this;
    }

    public InvalidableMetricRecordBuilder WithUnits(string? units)
    {
        _record = new InvalidableMetricRecord
        {
            Name = _record.Name,
            LogTimeUtc = _record.LogTimeUtc,
            CommitGitHubUrl = _record.CommitGitHubUrl,
            CommitMessage = _record.CommitMessage,
            Value = _record.Value,
            Units = units
        };

        return this;
    }

    public InvalidableMetricRecord Build() => _record;
}