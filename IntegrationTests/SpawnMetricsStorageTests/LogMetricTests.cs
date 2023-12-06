using System.Net;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace IntegrationTests.SpawnMetricsStorageTests;

public class LogMetricTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public async Task LogMetric_WithEmptyBody_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, null);

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithWrongBodyType_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, "WRONG");

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullProjectName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = null,
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = new string('A', ProjectNameConstants.MinProjectNameLength - 1),
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = new string('A', ProjectNameConstants.MaxProjectNameLength + 1),
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = null
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullMetricName_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = null,
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = new string('A', MetricRecordConstants.MinMetricNameLength - 1),
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = new string('A', MetricRecordConstants.MaxMetricNameLength + 1),
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = null,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                CommitGitHubUrl = "https://goooooooooooooooooooooogle.com",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlNullShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlTooShortShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                CommitGitHubUrl = $"https://github.com/spawn/spawn/commit/{new string('A', MetricRecordConstants.ShortCommitHashLength - 1)}",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlTooLongShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                CommitGitHubUrl = $"https://github.com/spawn/spawn/commit/{new string('A', MetricRecordConstants.ShortCommitHashLength + 1)}",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = "TEST"
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlInvalidShortCommitHash_ReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                CommitGitHubUrl = $"https://github.com/spawn/spawn/commit/{new string('_', MetricRecordConstants.ShortCommitHashLength)}",
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
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
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = new InvalidableMetricRecord
            {
                Name = "TEST",
                LogTimeUtc = DateTime.UtcNow,
                CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
                CommitMessage = "TEST",
                Value = "TEST",
                Units = new string('A', MetricRecordConstants.MinStringLength - 1)
            }
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists()
    {
        var testMetricRecord = new MetricRecord("TEST", DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(1));

        Assert.That(list[0], Is.EqualTo(testMetricRecord));
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_WhenTwoDifferentMetricsLogged()
    {
        var testMetricRecord = new MetricRecord("TEST", DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var testMetricRecord2 = new MetricRecord("TEST_ANOTHER", DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request2 = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord2
        });

        Assert.That(request2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(2));

        Assert.That(list.Any(metric => Equals(metric, testMetricRecord)));
        Assert.That(list.Any(metric => Equals(metric, testMetricRecord2)));
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_WhenSameMetricLoggedTwice()
    {
        var testMetricRecord = new MetricRecord("TEST", DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var testMetricRecord2 = new MetricRecord("TEST", DateTime.UtcNow + TimeSpan.FromMinutes(1), "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request2 = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord2
        });

        Assert.That(request2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(2));

        Assert.That(list.Any(metric => Equals(metric, testMetricRecord)));
        Assert.That(list.Any(metric => Equals(metric, testMetricRecord2)));
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_InDifferentProjects()
    {
        var testMetricRecord = new MetricRecord("TEST", DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var testMetricRecord2 = new MetricRecord("TEST", DateTime.UtcNow, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

        var request2 = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST_ANOTHER",
            Metric = testMetricRecord2
        });

        Assert.That(request2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(1));

        Assert.That(list[0], Is.EqualTo(testMetricRecord));

        var queryResponse2 = await _surrealDbClient.Query("SELECT * FROM TEST_ANOTHER");
        var list2 = queryResponse2.GetValue<List<MetricRecord>>(0);

        Assert.That(list2, Is.Not.Null);
        Assert.That(list2!.Count, Is.EqualTo(1));

        Assert.That(list2[0], Is.EqualTo(testMetricRecord2));
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
        public string? CommitGitHubUrl { get; init; }
        public string? CommitMessage { get; init; }
        public string? Value { get; init; }
        public string? Units { get; init; }
    }
}