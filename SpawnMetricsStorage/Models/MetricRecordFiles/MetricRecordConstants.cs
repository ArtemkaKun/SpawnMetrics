namespace SpawnMetricsStorage.Models.MetricRecordFiles;

public static class MetricRecordConstants
{
    public const int MinMetricNameLength = 3;
    public const int MaxMetricNameLength = 100;

    public const int ShortCommitHashLength = 7;

    public const int CommitGitHubUrlLength = 43;
    public const string CommitGitHubUrlShorterErrorMessage = "GitHub commit URL can't be shorter than 43 characters since it's always \'https://github.com/vlang/v/commit/\' + 7 characters of short commit hash";
    public const string CommitGitHubUrlLongerErrorMessage = "GitHub commit URL can't be longer than 43 characters since it's always \'https://github.com/vlang/v/commit/\' + 7 characters of short commit hash";

    public const int MinStringLength = 1;

    public const int MaxCommitMessageLength = 500;

    public const int MaxUnitsLength = 50;
}