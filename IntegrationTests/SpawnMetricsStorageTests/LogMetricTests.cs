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
        await TestLogMetricRequestWithBadProjectName(null);
    }

    [Test]
    public async Task LogMetric_WithEmptyProjectName_ReturnsBadRequest()
    {
        await TestLogMetricRequestWithBadProjectName("");
    }

    [Test]
    public async Task LogMetric_WithTooShortProjectName_ReturnsBadRequest()
    {
        await TestLogMetricRequestWithBadProjectName(CreateTestString(ProjectNameConstants.MinProjectNameLength - 1));
    }

    [Test]
    public async Task LogMetric_WithTooLongProjectName_ReturnsBadRequest()
    {
        await TestLogMetricRequestWithBadProjectName(CreateTestString(ProjectNameConstants.MaxProjectNameLength + 1));
    }

    private async Task TestLogMetricRequestWithBadProjectName(string? projectName)
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = projectName,
            Metric = new InvalidableMetricRecordBuilder().Build()
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_WithNullMetric_ReturnsBadRequest()
    {
        await TestLogMetricRequestWithBadMetricRecord(null);
    }

    [Test]
    public async Task LogMetric_WithNullMetricName_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName(null).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithEmptyMetricName_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName("").Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooShortMetricName_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName(CreateTestString(MetricRecordConstants.MinMetricNameLength - 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooLongMetricName_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName(CreateTestString(MetricRecordConstants.MaxMetricNameLength + 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNullMetricLogTime_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithLogTimeUtc(null).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNullCommitGitHubUrl_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(null).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooShortCommitGitHubUrl_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(CreateTestString(MetricRecordConstants.MinCommitGitHubUrlLength - 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooLongCommitGitHubUrl_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(CreateTestString(MetricRecordConstants.MaxCommitGitHubUrlLength + 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNotALinkCommitGitHubUrl_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(CreateTestString(MetricRecordConstants.MaxCommitGitHubUrlLength)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNotAGitHubLinkCommitGitHubUrl_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl("https://goooooooooooooooooooooogle.com").Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlNullShortCommitHash_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl("https://github.com/spawn/spawn/commit").Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlTooShortShortCommitHash_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl($"https://github.com/spawn/spawn/commit/{CreateTestString(MetricRecordConstants.ShortCommitHashLength - 1)}").Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlTooLongShortCommitHash_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl($"https://github.com/spawn/spawn/commit/{CreateTestString(MetricRecordConstants.ShortCommitHashLength + 1)}").Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithCommitGitHubUrlInvalidShortCommitHash_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl($"https://github.com/spawn/spawn/commit/{CreateTestString(MetricRecordConstants.ShortCommitHashLength, '_')}").Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNullCommitMessage_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitMessage(null).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooShortCommitMessage_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitMessage(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooLongCommitMessage_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitMessage(CreateTestString(MetricRecordConstants.MaxCommitMessageLength + 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNullValue_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithValue(null).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooShortValue_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithValue(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithNullUnits_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithUnits(null).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooShortUnits_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithUnits(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public async Task LogMetric_WithTooLongUnits_ReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithUnits(CreateTestString(MetricRecordConstants.MaxUnitsLength + 1)).Build();

        await TestLogMetricRequestWithBadMetricRecord(metric);
    }

    private async Task TestLogMetricRequestWithBadMetricRecord(InvalidableMetricRecord? metricRecord)
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = "TEST",
            Metric = metricRecord
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

    private static string CreateTestString(int length, char mainChar = 'A')
    {
        return new string(mainChar, length);
    }

    private class InvalidableLogMetricRequestBody
    {
        public string? ProjectName { get; init; }

        public InvalidableMetricRecord? Metric { get; init; }
    }
}