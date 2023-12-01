using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;

namespace IntegrationTests.SpawnMetricsStorageTests;

public class LogMetricTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public async Task LogMetric_WithEmptyBody_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, null);

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithWrongBodyType_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, "WRONG");

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullProjectName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = null,
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithEmptyProjectName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortProjectName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = new string('A', ProjectNameConstants.MinProjectNameLength - 1),
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooLongProjectName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = new string('A', ProjectNameConstants.MaxProjectNameLength + 1),
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullMetric_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = null
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullMetricName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = null,
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }
    
    [Test]
    public async Task LogMetric_WithEmptyMetricName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "",
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortMetricName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = new string('A', MetricRecordConstants.MinMetricNameLength - 1),
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooLongMetricName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = new string('A', MetricRecordConstants.MaxMetricNameLength + 1),
                LogTime = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }
    
    [Test]
    public async Task LogMetric_WithNullMetricLogTime_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTime = null,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    private class InvalidableLogMetricRequestBody
    {
        public string? ProjectName { get; init; }

        public InvalidableMetricRecord? Metric { get; init; }
    }

    private class InvalidableMetricRecord
    {
        public string? Name { get; init; }
        public DateTime? LogTime { get; init; }
        public string? ShortCommitHash { get; init; }
        public string? CommitGitHubUrl { get; init; }
        public string? CommitMessage { get; init; }
        public string? Value { get; init; }
        public string? Units { get; init; }
    }
}