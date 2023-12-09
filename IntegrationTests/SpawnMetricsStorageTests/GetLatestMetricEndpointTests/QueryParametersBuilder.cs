using SpawnMetricsStorage.Models;

namespace IntegrationTests.SpawnMetricsStorageTests.GetLatestMetricEndpointTests;

public sealed class QueryParametersBuilder
{
    private readonly Dictionary<string, string> _parameters = new()
    {
        { nameof(GetMetricRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
        { nameof(GetMetricRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName }
    };

    public QueryParametersBuilder WithProjectName(string projectName)
    {
        _parameters[nameof(GetMetricRequestParameters.ProjectName)] = projectName;

        return this;
    }

    public QueryParametersBuilder WithMetricName(string metricName)
    {
        _parameters[nameof(GetMetricRequestParameters.MetricName)] = metricName;

        return this;
    }

    public Dictionary<string, string> Build()
    {
        return _parameters;
    }
}