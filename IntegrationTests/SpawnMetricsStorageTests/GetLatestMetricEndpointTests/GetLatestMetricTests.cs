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
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyProjectNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), "" },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooShortProjectNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), new string('A', ProjectNameConstants.MinProjectNameLength - 1) },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task TooLongProjectNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), new string('A', ProjectNameConstants.MaxProjectNameLength + 1) },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task NoMetricNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    public Task EmptyMetricNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), "" }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    [NonParallelizable]
    public Task TooShortMetricNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), new string('A', MetricRecordConstants.MinStringLength - 1) }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    [NonParallelizable]
    public Task TooLongMetricNameParameterReturnsError()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), new string('A', MetricRecordConstants.MaxMetricNameLength + 1) }
        };

        var request = GetAsync(MetricsControllerConstants.LatestMetricEndpoint, parameters);

        return DoRequestAndAssertBadRequest(request);
    }

    [Test]
    [NonParallelizable]
    public Task EmptyDatabaseReturnsEmptyLatestMetric()
    {
        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        return RequestLatestMetricAndCheckResults(null, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task SingleMetricTableReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();

        await SurrealDbClient.Create(TestProjectName, testMetric);

        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleProjectsReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();

        await SurrealDbClient.Create(TestProjectName, testMetric);
        await SurrealDbClient.Create(AnotherProjectName, testMetric);

        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsInSameProjectReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();
        var latestTestMetric = CreateTestMetricRecord(AnotherMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await SurrealDbClient.Create(TestProjectName, testMetric);
        await SurrealDbClient.Create(TestProjectName, latestTestMetric);

        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task MultipleMetricsInDifferentProjectsReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();
        var latestTestMetric = CreateTestMetricRecord(TestMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await SurrealDbClient.Create(TestProjectName, testMetric);
        await SurrealDbClient.Create(AnotherProjectName, latestTestMetric);

        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

        await RequestLatestMetricAndCheckResults(testMetric, parameters);
    }

    [Test]
    [NonParallelizable]
    public async Task TwoSameMetricsReturnsLatestMetric()
    {
        var testMetric = CreateDefaultTestMetricRecord();
        var latestTestMetric = CreateTestMetricRecord(TestMetricName, DateTime.UtcNow + TimeSpan.FromMinutes(1));

        await SurrealDbClient.Create(TestProjectName, testMetric);
        await SurrealDbClient.Create(TestProjectName, latestTestMetric);

        var parameters = new Dictionary<string, string>
        {
            { nameof(GetMetricRequestParameters.ProjectName), TestProjectName },
            { nameof(GetMetricRequestParameters.MetricName), TestMetricName }
        };

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