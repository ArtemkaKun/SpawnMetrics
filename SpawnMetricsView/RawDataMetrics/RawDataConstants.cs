using MetricRecordModel;

namespace SpawnMetricsView.RawDataMetrics;

public static class RawDataConstants
{
    public const string LogTimePropertyName = nameof(MetricRecord.LogTimeUtc);
    public const string CommitGitHubUrlPropertyName = nameof(MetricRecord.CommitGitHubUrl);
    public const string CommitMessagePropertyName = nameof(MetricRecord.CommitMessage);
}