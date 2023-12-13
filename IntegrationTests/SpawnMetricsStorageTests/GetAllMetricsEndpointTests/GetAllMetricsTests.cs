using System.Text.Json;
using MetricRecordModel;
using SharedConstants;
using SpawnMetricsStorage.Controllers;

namespace IntegrationTests.SpawnMetricsStorageTests.GetAllMetricsEndpointTests;

public sealed class GetAllMetricsTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public Task NoParametersReturnsError()
    {
        var request = GetAsync(EndpointsConstants.GetAllMetricsEndpoint, null);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithoutProjectName().Build();
        var request = GetAsync(EndpointsConstants.GetAllMetricsEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName("").Build();
        var request = GetAsync(EndpointsConstants.GetAllMetricsEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooShortProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTooShortProjectName()).Build();
        var request = GetAsync(EndpointsConstants.GetAllMetricsEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTooLongProjectName()).Build();
        var request = GetAsync(EndpointsConstants.GetAllMetricsEndpoint, parameters);

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

        var parameters = new QueryParametersBuilder().Build();
        await RequestMetricDataRangeAndCheckResults(createdMetrics, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsReturnsMultipleMetrics()
    {
        const int metricsCount = 3;
        var timeNow = DateTime.UtcNow;
        var createdMetrics = await CreateMultipleMetricsWithLogTimeShift(metricsCount, timeNow);

        var parameters = new QueryParametersBuilder().Build();
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

        var parameters = new QueryParametersBuilder().Build();
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
        var request = GetAsync(EndpointsConstants.GetAllMetricsEndpoint, queryParameters);

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