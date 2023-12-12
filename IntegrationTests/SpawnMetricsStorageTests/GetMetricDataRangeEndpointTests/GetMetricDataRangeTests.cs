using System.Text.Json;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models.MetricRecordFiles;

namespace IntegrationTests.SpawnMetricsStorageTests.GetMetricDataRangeEndpointTests;

public sealed class GetMetricDataRangeTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public Task NoParametersReturnsError()
    {
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, null);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithoutProjectName().Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName("").Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooShortProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTooShortProjectName()).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTooLongProjectName()).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithoutMetricName().Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName("").Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooShortMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName(CreateTooShortMetricName()).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName(CreateTooLongMetricName()).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoRangeStartParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithoutRangeStart().Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task RangeStartParameterLaterThanRangeEndReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithRangeStart(DateTime.UtcNow + TimeSpan.FromHours(1)).WithRangeEnd(DateTime.UtcNow).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoRangeEndParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithoutRangeEnd().Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task RangeEndParameterEarlierThanRangeStartReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithRangeStart(DateTime.UtcNow).WithRangeEnd(DateTime.UtcNow - TimeSpan.FromHours(1)).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    [NonParallelizable]
    public Task EmptyDatabaseReturnsEmptyLatestMetric()
    {
        var parameters = new QueryParametersBuilder().Build();
        return RequestMetricDataRangeAndCheckResults(null, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task SingleMetricReturnsSingleMetric()
    {
        const int metricsCount = 1;
        var timeNow = DateTime.UtcNow;
        var createdMetrics = await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow - TimeSpan.FromMinutes(1)).WithRangeEnd(timeNow + TimeSpan.FromMinutes(metricsCount)).Build();
        await RequestMetricDataRangeAndCheckResults(createdMetrics, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsReturnsMultipleMetrics()
    {
        const int metricsCount = 3;
        var timeNow = DateTime.UtcNow;
        var createdMetrics = await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow - TimeSpan.FromMinutes(1)).WithRangeEnd(timeNow + TimeSpan.FromMinutes(metricsCount)).Build();
        await RequestMetricDataRangeAndCheckResults(createdMetrics, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsOnRangeBordersReturnsMultipleMetrics()
    {
        const int metricsCount = 3;
        var timeNow = DateTime.UtcNow;
        var createdMetrics = await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow).WithRangeEnd(timeNow + TimeSpan.FromMinutes(metricsCount - 1)).Build();
        await RequestMetricDataRangeAndCheckResults(createdMetrics, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleDifferentMetricsReturnsMultipleMetrics()
    {
        const int metricsCount = 3;
        var timeNow = DateTime.UtcNow;

        var createdMetrics = await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow);
        await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow, SpawnMetricsStorageTestsConstants.AnotherMetricName);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow).WithRangeEnd(timeNow + TimeSpan.FromMinutes(metricsCount)).Build();
        await RequestMetricDataRangeAndCheckResults(createdMetrics, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsInDifferentProjectsReturnsMultipleMetrics()
    {
        const int metricsCount = 3;
        var timeNow = DateTime.UtcNow;

        var createdMetrics = await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow);
        await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow, projectName: SpawnMetricsStorageTestsConstants.AnotherProjectName);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow).WithRangeEnd(timeNow + TimeSpan.FromMinutes(metricsCount)).Build();
        await RequestMetricDataRangeAndCheckResults(createdMetrics, parameters);
    }

    private async Task<List<MetricRecord>> CreateMultipleMetricsWithLogTimeShift(int countOfMetrics, DateTime currentTime, string metricName = SpawnMetricsStorageTestsConstants.TestMetricName,
        string projectName = SpawnMetricsStorageTestsConstants.TestProjectName)
    {
        var metrics = new List<MetricRecord>();

        for (var i = 0; i < countOfMetrics; i++)
        {
            var metric = CreateTestMetricRecord(metricName, currentTime + TimeSpan.FromMinutes(i));
            await SurrealDbClient.Create(projectName, metric);

            metrics.Add(metric);
        }

        return metrics;
    }

    private async Task RequestMetricDataRangeAndCheckResults(List<MetricRecord>? expectedMetric, Dictionary<string, string>? queryParameters)
    {
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, queryParameters);

        await DoRequestAndAssertOk(request);

        var response = await request;
        var responseContent = await response.Content.ReadAsStringAsync();

        var latestMetric = string.IsNullOrEmpty(responseContent)
            ? null
            : JsonSerializer.Deserialize<List<MetricRecord>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (latestMetric == null)
        {
            Assert.That(expectedMetric, Is.Null);
            return;
        }

        if (expectedMetric == null)
        {
            Assert.That(latestMetric, Is.Null);
            return;
        }

        Assert.That(latestMetric, Is.EquivalentTo(expectedMetric));
    }
}