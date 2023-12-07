using System.Net;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace IntegrationTests.SpawnMetricsStorageTests;

public class LogMetricTests : SpawnMetricsStorageTestsBase
{
    private const string TestProjectName = "TEST";
    private const string TestMetricName = "TEST";

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

    private static string CreateTestString(int length, char mainChar = 'A')
    {
        return new string(mainChar, length);
    }

    private async Task TestLogMetricRequestWithBadMetricRecord(InvalidableMetricRecord? metricRecord)
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new InvalidableLogMetricRequestBody
        {
            ProjectName = TestProjectName,
            Metric = metricRecord
        });

        await DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord]);
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_WhenTwoDifferentMetricsLogged()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var testMetricRecord2 = CreateTestMetricRecord("TEST_ANOTHER", DateTime.UtcNow);

        await LogCorrectMetric(testMetricRecord2);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord, testMetricRecord2]);
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_WhenSameMetricLoggedTwice()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var testMetricRecord2 = CreateTestMetricRecord(TestMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await LogCorrectMetric(testMetricRecord2);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord, testMetricRecord2]);
    }

    [Test]
    public async Task LogMetric_ReturnOk_AndLogMetricExists_InDifferentProjects()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var testMetricRecord2 = CreateDefaultTestMetricRecord();

        const string anotherProjectName = "TEST_ANOTHER";

        await LogCorrectMetric(testMetricRecord2, anotherProjectName);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord]);

        var loggedMetrics2 = await GetLoggedMetrics(anotherProjectName);

        AssertExpectedLoggedMetrics(loggedMetrics2!, [testMetricRecord2]);
    }

    private static MetricRecord CreateDefaultTestMetricRecord() => CreateTestMetricRecord(TestMetricName, DateTime.UtcNow);

    private static MetricRecord CreateTestMetricRecord(string metricName, DateTime logTime) => new(metricName, logTime, "https://github.com/spawn/spawn/commit/12345678", "TEST", "TEST", "TEST");

    private async Task LogCorrectMetric(MetricRecord correctMetricRecord, string projectName = TestProjectName)
    {
        var request = await PutAsync(MetricsControllerConstants.MetricEndpoint, new LogMetricRequestBody
        {
            ProjectName = projectName,
            Metric = correctMetricRecord
        });

        Assert.That(request.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task<List<MetricRecord>?> GetLoggedMetrics(string table = TestProjectName)
    {
        var queryResponse = await SurrealDbClient.Query($"SELECT * FROM {table}");

        return queryResponse.GetValue<List<MetricRecord>>(0);
    }

    private static void AssertExpectedLoggedMetrics(List<MetricRecord> loggedMetrics, List<MetricRecord> expectedLoggedMetrics)
    {
        Assert.Multiple(() =>
        {
            Assert.That(CheckMetricsNotNullAndHasExpectedCount(loggedMetrics, expectedLoggedMetrics.Count));
            Assert.That(loggedMetrics.All(metric => expectedLoggedMetrics.Any(expectedMetric => Equals(metric, expectedMetric))));
        });
    }

    private static bool CheckMetricsNotNullAndHasExpectedCount(List<MetricRecord>? metrics, int expectedCount)
    {
        return metrics != null && metrics.Count == expectedCount;
    }

    private class InvalidableLogMetricRequestBody
    {
        public string? ProjectName { get; init; }

        public InvalidableMetricRecord? Metric { get; init; }
    }
}