using System.Text.Json;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Models.ProjectName;

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
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricDataRangeRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName },
            { nameof(GetMetricDataRangeRequestParameters.RangeStart), DateTime.UtcNow.ToString("O") },
            { nameof(GetMetricDataRangeRequestParameters.RangeEnd), (DateTime.UtcNow + TimeSpan.FromHours(1)).ToString("O") }
        };

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
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTestString(ProjectNameConstants.MinProjectNameLength - 1)).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongProjectNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithProjectName(CreateTestString(ProjectNameConstants.MaxProjectNameLength + 1)).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoMetricNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
            { nameof(GetMetricDataRangeRequestParameters.RangeStart), DateTime.UtcNow.ToString("O") },
            { nameof(GetMetricDataRangeRequestParameters.RangeEnd), (DateTime.UtcNow + TimeSpan.FromHours(1)).ToString("O") }
        };

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
        var parameters = new QueryParametersBuilder().WithMetricName(CreateTestString(MetricRecordConstants.MinStringLength - 1)).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongMetricNameParameterReturnsError()
    {
        var parameters = new QueryParametersBuilder().WithMetricName(CreateTestString(MetricRecordConstants.MaxMetricNameLength + 1)).Build();
        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoRangeStartParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
            { nameof(GetMetricDataRangeRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName },
            { nameof(GetMetricDataRangeRequestParameters.RangeEnd), (DateTime.UtcNow + TimeSpan.FromHours(1)).ToString("O") }
        };

        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task RangeStartParameterLaterThanRangeEndReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
            { nameof(GetMetricDataRangeRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName },
            { nameof(GetMetricDataRangeRequestParameters.RangeStart), (DateTime.UtcNow + TimeSpan.FromHours(1)).ToString("O") },
            { nameof(GetMetricDataRangeRequestParameters.RangeEnd), DateTime.UtcNow.ToString("O") }
        };

        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoRangeEndParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
            { nameof(GetMetricDataRangeRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName },
            { nameof(GetMetricDataRangeRequestParameters.RangeStart), DateTime.UtcNow.ToString("O") }
        };

        var request = GetAsync(MetricsControllerConstants.MetricDataRangeEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task RangeEndParameterEarlierThanRangeStartReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
            { nameof(GetMetricDataRangeRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName },
            { nameof(GetMetricDataRangeRequestParameters.RangeStart), DateTime.UtcNow.ToString("O") },
            { nameof(GetMetricDataRangeRequestParameters.RangeEnd), (DateTime.UtcNow - TimeSpan.FromHours(1)).ToString("O") }
        };

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
        var testMetric = CreateDefaultTestMetricRecord();

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);

        var parameters = new QueryParametersBuilder().WithRangeStart(DateTime.UtcNow - TimeSpan.FromMinutes(1)).WithRangeEnd(DateTime.UtcNow + TimeSpan.FromMinutes(1)).Build();
        await RequestMetricDataRangeAndCheckResults([testMetric], parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsReturnsMultipleMetrics()
    {
        var timeNow = DateTime.UtcNow;

        var testMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow);
        var anotherMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(1));
        var anotherMetric2 = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(2));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric2);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow - TimeSpan.FromMinutes(1)).WithRangeEnd(timeNow + TimeSpan.FromMinutes(3)).Build();
        await RequestMetricDataRangeAndCheckResults([testMetric, anotherMetric, anotherMetric2], parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsOnRangeBordersReturnsMultipleMetrics()
    {
        var timeNow = DateTime.UtcNow;

        var testMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow);
        var anotherMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(1));
        var anotherMetric2 = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(2));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric2);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow).WithRangeEnd(timeNow + TimeSpan.FromMinutes(2)).Build();
        await RequestMetricDataRangeAndCheckResults([testMetric, anotherMetric, anotherMetric2], parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleDifferentMetricsReturnsMultipleMetrics()
    {
        var timeNow = DateTime.UtcNow;

        var testMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow);
        var anotherMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(1));
        var anotherMetric2 = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(2));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric2);

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.AnotherMetricName, timeNow));
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.AnotherMetricName, timeNow + TimeSpan.FromMinutes(1)));
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.AnotherMetricName, timeNow + TimeSpan.FromMinutes(2)));

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow).WithRangeEnd(timeNow + TimeSpan.FromMinutes(2)).Build();
        await RequestMetricDataRangeAndCheckResults([testMetric, anotherMetric, anotherMetric2], parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsInDifferentProjectsReturnsMultipleMetrics()
    {
        var timeNow = DateTime.UtcNow;

        var testMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow);
        var anotherMetric = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(1));
        var anotherMetric2 = CreateTestMetricRecord(SpawnMetricsStorageTestsConstants.TestMetricName, timeNow + TimeSpan.FromMinutes(2));

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.TestProjectName, anotherMetric2);

        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.AnotherProjectName, testMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.AnotherProjectName, anotherMetric);
        await SurrealDbClient.Create(SpawnMetricsStorageTestsConstants.AnotherProjectName, anotherMetric2);

        var parameters = new QueryParametersBuilder().WithRangeStart(timeNow).WithRangeEnd(timeNow + TimeSpan.FromMinutes(2)).Build();
        await RequestMetricDataRangeAndCheckResults([testMetric, anotherMetric, anotherMetric2], parameters);
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