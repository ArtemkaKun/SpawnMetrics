using JetBrains.Annotations;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace IntegrationTests.SpawnMetricsStorageTests.LogMetricEndpointTests;

[NonParallelizable]
public sealed class LogMetricTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public Task NoAuthKeyProvidedReturnsUnauthorized()
    {
        var body = new LogMetricRequestBody
        {
            ProjectName = TestProjectName,
            Metric = CreateDefaultTestMetricRecord()
        };

        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, null, body);

        return DoRequestAndAssertUnauthorized(request);
    }

    [Test]
    public Task WrongAuthKeyProvidedReturnsUnauthorized()
    {
        var body = new LogMetricRequestBody
        {
            ProjectName = TestProjectName,
            Metric = CreateDefaultTestMetricRecord()
        };

        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, new Dictionary<string, string> { { MetricsControllerConstants.ApiKeyParameter, "WRONG KEY" } }, body);

        return DoRequestAndAssertUnauthorized(request);
    }


    [Test]
    public Task EmptyBodyReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, CreateHeadersWithApiKey(), null);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task WrongBodyTypeReturnsBadRequest()
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, CreateHeadersWithApiKey(), "WRONG");

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NullProjectNameReturnsBadRequest()
    {
        return TestLogMetricRequestWithBadProjectName(null);
    }

    [Test]
    public Task EmptyProjectNameReturnsBadRequest()
    {
        return TestLogMetricRequestWithBadProjectName("");
    }

    [Test]
    public Task TooShortProjectNameReturnsBadRequest()
    {
        return TestLogMetricRequestWithBadProjectName(CreateTestString(ProjectNameConstants.MinProjectNameLength - 1));
    }

    [Test]
    public Task TooLongProjectNameReturnsBadRequest()
    {
        return TestLogMetricRequestWithBadProjectName(CreateTestString(ProjectNameConstants.MaxProjectNameLength + 1));
    }

    private Task TestLogMetricRequestWithBadProjectName(string? projectName)
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, CreateHeadersWithApiKey(), new InvalidableLogMetricRequestBody(projectName, new InvalidableMetricRecordBuilder().Build()));

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NullMetricReturnsBadRequest()
    {
        return TestLogMetricRequestWithBadMetricRecord(null);
    }

    [Test]
    public Task NullMetricNameReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName(null).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task EmptyMetricNameReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName("").Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooShortMetricNameReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName(CreateTestString(MetricRecordConstants.MinMetricNameLength - 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooLongMetricNameReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithName(CreateTestString(MetricRecordConstants.MaxMetricNameLength + 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NullMetricLogTimeReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithLogTimeUtc(null).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NullCommitGitHubUrlReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(null).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooShortCommitGitHubUrlReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(CreateTestString(MetricRecordConstants.MinCommitGitHubUrlLength - 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooLongCommitGitHubUrlReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(CreateTestString(MetricRecordConstants.MaxCommitGitHubUrlLength + 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NotALinkCommitGitHubUrlReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl(CreateTestString(MetricRecordConstants.MaxCommitGitHubUrlLength)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NotAGitHubLinkCommitGitHubUrlReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl("https://goooooooooooooooooooooogle.com").Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task CommitGitHubUrlNullShortCommitHashReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl("https://github.com/spawn/spawn/commit").Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task CommitGitHubUrlTooShortShortCommitHashReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl($"https://github.com/spawn/spawn/commit/{CreateTestString(MetricRecordConstants.ShortCommitHashLength - 1)}").Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task CommitGitHubUrlTooLongShortCommitHashReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl($"https://github.com/spawn/spawn/commit/{CreateTestString(MetricRecordConstants.ShortCommitHashLength + 1)}").Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task CommitGitHubUrlInvalidShortCommitHashReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitGitHubUrl($"https://github.com/spawn/spawn/commit/{CreateTestString(MetricRecordConstants.ShortCommitHashLength, '_')}").Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NullCommitMessageReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitMessage(null).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooShortCommitMessageReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitMessage(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooLongCommitMessageReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithCommitMessage(CreateTestString(MetricRecordConstants.MaxCommitMessageLength + 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NullValueReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithValue(null).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooShortValueReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithValue(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task NullUnitsReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithUnits(null).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooShortUnitsReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithUnits(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    [Test]
    public Task TooLongUnitsReturnsBadRequest()
    {
        var metric = new InvalidableMetricRecordBuilder().WithUnits(CreateTestString(MetricRecordConstants.MaxUnitsLength + 1)).Build();

        return TestLogMetricRequestWithBadMetricRecord(metric);
    }

    private Task TestLogMetricRequestWithBadMetricRecord(InvalidableMetricRecord? metricRecord)
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, CreateHeadersWithApiKey(), new InvalidableLogMetricRequestBody(TestProjectName, metricRecord));

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    [NonParallelizable]
    public async Task CorrectMetricReturnsOkAndExistsInDb()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord]);
    }

    [Test]
    [NonParallelizable]
    public async Task TwoDifferentCorrectMetricsReturnsOkAndExistInDb()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var testMetricRecord2 = CreateTestMetricRecord(AnotherMetricName, DateTime.UtcNow);

        await LogCorrectMetric(testMetricRecord2);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord, testMetricRecord2]);
    }

    [Test]
    [NonParallelizable]
    public async Task SameMetricLoggedTwiceReturnsOkAndExistsInDb()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var testMetricRecord2 = CreateTestMetricRecord(TestMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await LogCorrectMetric(testMetricRecord2);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord, testMetricRecord2]);
    }

    [Test]
    [NonParallelizable]
    public async Task CorrectMetricsFromDifferentProjectsReturnsOkAndExistInDb()
    {
        var testMetricRecord = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord);

        var testMetricRecord2 = CreateDefaultTestMetricRecord();

        await LogCorrectMetric(testMetricRecord2, AnotherProjectName);

        var loggedMetrics = await GetLoggedMetrics();

        AssertExpectedLoggedMetrics(loggedMetrics!, [testMetricRecord]);

        var loggedMetrics2 = await GetLoggedMetrics(AnotherProjectName);

        AssertExpectedLoggedMetrics(loggedMetrics2!, [testMetricRecord2]);
    }

    private Task LogCorrectMetric(MetricRecord correctMetricRecord, string projectName = TestProjectName)
    {
        var request = PutAsync(MetricsControllerConstants.MetricEndpoint, CreateHeadersWithApiKey(), new LogMetricRequestBody
        {
            ProjectName = projectName,
            Metric = correctMetricRecord
        });

        return DoRequestAndAssertOk(request);
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

    private readonly struct InvalidableLogMetricRequestBody(string? projectName, InvalidableMetricRecord? metric)
    {
        [UsedImplicitly]
        public readonly string? ProjectName = projectName;

        [UsedImplicitly]
        public readonly InvalidableMetricRecord? Metric = metric;
    }
}