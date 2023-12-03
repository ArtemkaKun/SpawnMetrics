using System.Net;
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

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists()
    {
        var testMetricRecord = new MetricRecord
        {
            Name = "TEST",
            LogTimeUtc = DateTime.UtcNow,
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(1));

        var metric = list[0];

        Assert.That(metric.Name, Is.EqualTo(testMetricRecord.Name));
        Assert.That(metric.LogTimeUtc, Is.EqualTo(testMetricRecord.LogTimeUtc));
        Assert.That(metric.ShortCommitHash, Is.EqualTo(testMetricRecord.ShortCommitHash));
        Assert.That(metric.CommitGitHubUrl, Is.EqualTo(testMetricRecord.CommitGitHubUrl));
        Assert.That(metric.CommitMessage, Is.EqualTo(testMetricRecord.CommitMessage));
        Assert.That(metric.Value, Is.EqualTo(testMetricRecord.Value));
        Assert.That(metric.Units, Is.EqualTo(testMetricRecord.Units));
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_WhenTwoDifferentMetricsLogged()
    {
        var testMetricRecord = new MetricRecord
        {
            Name = "TEST",
            LogTimeUtc = DateTime.UtcNow,
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var testMetricRecord2 = new MetricRecord
        {
            Name = "TEST_ANOTHER",
            LogTimeUtc = DateTime.UtcNow,
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request2 = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord2
        });

        Assert.That(request2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(2));

        Assert.That(list.Any(metric => metric.Name == testMetricRecord.Name));
        Assert.That(list.Any(metric => metric.LogTimeUtc == testMetricRecord.LogTimeUtc));
        Assert.That(list.Any(metric => metric.ShortCommitHash == testMetricRecord.ShortCommitHash));
        Assert.That(list.Any(metric => metric.CommitGitHubUrl == testMetricRecord.CommitGitHubUrl));
        Assert.That(list.Any(metric => metric.CommitMessage == testMetricRecord.CommitMessage));
        Assert.That(list.Any(metric => metric.Value == testMetricRecord.Value));
        Assert.That(list.Any(metric => metric.Units == testMetricRecord.Units));
        
        Assert.That(list.Any(metric => metric.Name == testMetricRecord2.Name));
        Assert.That(list.Any(metric => metric.LogTimeUtc == testMetricRecord2.LogTimeUtc));
        Assert.That(list.Any(metric => metric.ShortCommitHash == testMetricRecord2.ShortCommitHash));
        Assert.That(list.Any(metric => metric.CommitGitHubUrl == testMetricRecord2.CommitGitHubUrl));
        Assert.That(list.Any(metric => metric.CommitMessage == testMetricRecord2.CommitMessage));
        Assert.That(list.Any(metric => metric.Value == testMetricRecord2.Value));
        Assert.That(list.Any(metric => metric.Units == testMetricRecord2.Units));
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_WhenSameMetricLoggedTwice()
    {
        var testMetricRecord = new MetricRecord
        {
            Name = "TEST",
            LogTimeUtc = DateTime.UtcNow,
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var testMetricRecord2 = new MetricRecord
        {
            Name = "TEST",
            LogTimeUtc = DateTime.UtcNow + TimeSpan.FromMinutes(1),
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request2 = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord2
        });

        Assert.That(request2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(2));

        Assert.That(list.Any(metric => metric.Name == testMetricRecord.Name));
        Assert.That(list.Any(metric => metric.LogTimeUtc == testMetricRecord.LogTimeUtc));
        Assert.That(list.Any(metric => metric.ShortCommitHash == testMetricRecord.ShortCommitHash));
        Assert.That(list.Any(metric => metric.CommitGitHubUrl == testMetricRecord.CommitGitHubUrl));
        Assert.That(list.Any(metric => metric.CommitMessage == testMetricRecord.CommitMessage));
        Assert.That(list.Any(metric => metric.Value == testMetricRecord.Value));
        Assert.That(list.Any(metric => metric.Units == testMetricRecord.Units));
        
        Assert.That(list.Any(metric => metric.Name == testMetricRecord2.Name));
        Assert.That(list.Any(metric => metric.LogTimeUtc == testMetricRecord2.LogTimeUtc));
        Assert.That(list.Any(metric => metric.ShortCommitHash == testMetricRecord2.ShortCommitHash));
        Assert.That(list.Any(metric => metric.CommitGitHubUrl == testMetricRecord2.CommitGitHubUrl));
        Assert.That(list.Any(metric => metric.CommitMessage == testMetricRecord2.CommitMessage));
        Assert.That(list.Any(metric => metric.Value == testMetricRecord2.Value));
        Assert.That(list.Any(metric => metric.Units == testMetricRecord2.Units));
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_InDifferentProjects()
    {
        var testMetricRecord = new MetricRecord
        {
            Name = "TEST",
            LogTimeUtc = DateTime.UtcNow,
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = testMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var testMetricRecord2 = new MetricRecord
        {
            Name = "TEST",
            LogTimeUtc = DateTime.UtcNow + TimeSpan.FromMinutes(1),
            ShortCommitHash = "12345678",
            CommitGitHubUrl = "https://github.com/spawn/spawn/commit/12345678",
            CommitMessage = "TEST",
            Value = "TEST",
            Units = "TEST"
        };

        var request2 = await PutAsync(MetricsControllerConstants.LogMetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = "TEST_ANOTHER",
            Metric = testMetricRecord2
        });

        Assert.That(request2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var queryResponse = await _surrealDbClient.Query("SELECT * FROM TEST");
        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        Assert.That(list, Is.Not.Null);
        Assert.That(list!.Count, Is.EqualTo(1));

        var metric = list[0];

        Assert.That(metric.Name, Is.EqualTo(testMetricRecord.Name));
        Assert.That(metric.LogTimeUtc, Is.EqualTo(testMetricRecord.LogTimeUtc));
        Assert.That(metric.ShortCommitHash, Is.EqualTo(testMetricRecord.ShortCommitHash));
        Assert.That(metric.CommitGitHubUrl, Is.EqualTo(testMetricRecord.CommitGitHubUrl));
        Assert.That(metric.CommitMessage, Is.EqualTo(testMetricRecord.CommitMessage));
        Assert.That(metric.Value, Is.EqualTo(testMetricRecord.Value));
        Assert.That(metric.Units, Is.EqualTo(testMetricRecord.Units));

        var queryResponse2 = await _surrealDbClient.Query("SELECT * FROM TEST_ANOTHER");
        var list2 = queryResponse2.GetValue<List<MetricRecord>>(0);

        Assert.That(list2, Is.Not.Null);
        Assert.That(list2!.Count, Is.EqualTo(1));

        var metric2 = list2[0];

        Assert.That(metric2.Name, Is.EqualTo(testMetricRecord2.Name));
        Assert.That(metric2.LogTimeUtc, Is.EqualTo(testMetricRecord2.LogTimeUtc));
        Assert.That(metric2.ShortCommitHash, Is.EqualTo(testMetricRecord2.ShortCommitHash));
        Assert.That(metric2.CommitGitHubUrl, Is.EqualTo(testMetricRecord2.CommitGitHubUrl));
        Assert.That(metric2.CommitMessage, Is.EqualTo(testMetricRecord2.CommitMessage));
        Assert.That(metric2.Value, Is.EqualTo(testMetricRecord2.Value));
        Assert.That(metric2.Units, Is.EqualTo(testMetricRecord2.Units));
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