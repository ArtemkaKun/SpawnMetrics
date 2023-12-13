namespace SpawnMetricsView.DataCollectorFiles;

public readonly struct CommitMetrics(DateTime logTimeUtc, string commitGitHubUrl, string commitMessage, List<CommitMetric> metric)
{
    public DateTime LogTimeUtc { get; } = logTimeUtc;
    public string CommitGitHubUrl { get; } = commitGitHubUrl;
    public string CommitMessage { get; } = commitMessage;
    public List<CommitMetric> Metric { get; } = metric;
}

public readonly struct CommitMetric(string name, string value, string units)
{
    public readonly string Name = name;
    public readonly string Value = value;
    public readonly string Units = units;
}