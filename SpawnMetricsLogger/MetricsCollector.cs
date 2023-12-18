using System.Diagnostics;
using LibGit2Sharp;
using MetricRecordModel;
using SharedConstants;
using SpawnMetricsLogger.Config;

namespace SpawnMetricsLogger;

public static class MetricsCollector
{
    public static List<MetricRecord>? CollectMetrics(Commit currentCommit, string repoPath, ConfigModel config)
    {
        var commitLogTimeUtc = currentCommit.Committer.When.UtcDateTime;
        var commitGitHubUrl = $"{config.BaseCommitGitHubUrl}{currentCommit.Sha[..GitConstants.ShortCommitHashLength]}";
        var metricRecords = new List<MetricRecord>();

        foreach (var metricOperation in config.MetricOperations)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "bash",
                WorkingDirectory = repoPath,
                Arguments = $"-c \"{metricOperation.Command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process == null)
            {
                Console.WriteLine($"Failed to start process for metric {metricOperation.Name}");
                return null;
            }

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Failed to get metric {metricOperation.Name}");
                return null;
            }

            var output = process.StandardOutput.ReadToEnd();

            if (string.IsNullOrWhiteSpace(output))
            {
                Console.WriteLine($"Failed to get metric {metricOperation.Name}");
                return null;
            }

            var metric = new MetricRecord(metricOperation.Name, commitLogTimeUtc, commitGitHubUrl, currentCommit.Message.Trim(), output.Trim(), metricOperation.Units);
            metricRecords.Add(metric);
        }

        return metricRecords;
    }
}