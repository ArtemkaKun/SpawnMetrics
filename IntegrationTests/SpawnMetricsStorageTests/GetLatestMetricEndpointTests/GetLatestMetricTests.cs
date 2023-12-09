using System.Text.Json;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

namespace IntegrationTests.SpawnMetricsStorageTests.GetLatestMetricEndpointTests;

[NonParallelizable]
public sealed class GetLatestMetricTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public Task NoParametersReturnsError()
    {
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, null);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoProjectNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName("").Build();
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooShortProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTestString(ProjectNameConstants.MinProjectNameLength - 1)).Build();
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTestString(ProjectNameConstants.MaxProjectNameLength + 1)).Build();
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoMetricNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName("").Build();
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooShortMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName(CreateTestString(MetricRecordConstants.MaxMetricNameLength + 1)).Build();
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    [NonParallelizable]
    public Task EmptyDatabaseReturnsEmptyLatestMetric()
    {
        var parameters = new QueryParametersBuilder().Build();
        return RequestLatestMetricAndCheckResults(null, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task SingleMetricTableReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);

        var parameters = new QueryParametersBuilder().Build();
        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleProjectsReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(AnotherProjectName, testMetric);

        var parameters = new QueryParametersBuilder().Build();
        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsInSameProjectReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();
        var latestTestMetric = CreateTestMetricRecord(AnotherMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, latestTestMetric);

        var parameters = new QueryParametersBuilder().Build();
        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsInDifferentProjectsReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();
        var latestTestMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(AnotherProjectName, latestTestMetric);

        var parameters = new QueryParametersBuilder().Build();
        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task TwoSameMetricsReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();
        var latestTestMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, latestTestMetric);

        var parameters = new QueryParametersBuilder().Build();
        await RequestLatestMetricAndCheckResults(latestTestMetric, parameters);
    }

    private async Task RequestLatestMetricAndCheckResults(MetricRecord? expectedMetric, Dictionary<string, string>? queryParameters)
    {
        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, queryParameters);

        await DoRequestAndAssertOk(request);

        var response = await request;
        var responseContent = await response.Content.ReadAsStringAsync();

        MetricRecord? latestMetric = string.IsNullOrEmpty(responseContent)
            ? null
            : JsonSerializer.Deserialize<MetricRecord>(responseContent, new JsonSerializerOptions
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

        Assert.That(latestMetric.Value, Is.EqualTo(expectedMetric.Value));
    }
}