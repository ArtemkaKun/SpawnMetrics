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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = DateTime.UtcNow,
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
                LogTimeUtc = null,
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
    public async Task LogMetric_WithNullShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = null,
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = new string('A', MetricRecordConstants.ShortCommitHashLength - 1),
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooLongShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = new string('A', MetricRecordConstants.ShortCommitHashLength + 1),
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithInvalidShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = new string('_', MetricRecordConstants.ShortCommitHashLength),
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullCommitGitHubUrl_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = null,
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortCommitGitHubUrl_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = new string('A', MetricRecordConstants.MinCommitGitHubUrlLength - 1),
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooLongCommitGitHubUrl_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = new string('A', MetricRecordConstants.MaxCommitGitHubUrlLength + 1),
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNotALinkCommitGitHubUrl_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = new string('A', MetricRecordConstants.MaxCommitGitHubUrlLength),
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNotAGitHubLinkCommitGitHubUrl_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://goooooooooooooooooooooogle.com",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullCommitMessage_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = null,
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortCommitMessage_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = new string('A', MetricRecordConstants.MinStringLength - 1),
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooLongCommitMessage_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = new string('A', MetricRecordConstants.MaxCommitMessageLength + 1),
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullValue_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = null,
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortValue_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = new string('A', MetricRecordConstants.MinStringLength - 1),
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullUnits_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = null
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithTooShortUnits_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.LogMetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                ShortCommitHash = "12345678",
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = new string('A', MetricRecordConstants.MinStringLength - 1)
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
        public DateTime? LogTimeUtc { get; init; }
        public string? ShortCommitHash { get; init; }
        public string? CommitGitHubUrl { get; init; }
        public string? CommitMessage { get; init; }
        public string? Value { get; init; }
        public string? Units { get; init; }
    }
}