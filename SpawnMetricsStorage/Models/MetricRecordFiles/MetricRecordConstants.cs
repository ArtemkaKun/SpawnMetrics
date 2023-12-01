namespace SpawnMetricsStorage.Models.MetricRecordFiles;

public static class MetricRecordConstants
{
    public const int MinMetricNameLength = 3;
    public const int MaxMetricNameLength = 100;

    public const int ShortCommitHashLength = 8;

    public const int MinCommitGitHubUrlLength = 37;
    public const int MaxCommitGitHubUrlLength = 100;
    public const string CommitGitHubUrlShorterErrorMessage = "GitHub commit URL can't be shorter than 37 characters since it's always \'https://github.com/*/*/commit/\' + 8 characters of short commit hash";

    public const int MinStringLength = 1;

    public const int MaxCommitMessageLength = 500;

    public const int MaxUnitsLength = 50;
}