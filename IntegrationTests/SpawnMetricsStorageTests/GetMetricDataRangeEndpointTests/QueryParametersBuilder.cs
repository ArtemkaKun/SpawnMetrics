using SpawnMetricsStorage.Models;

namespace IntegrationTests.SpawnMetricsStorageTests.GetMetricDataRangeEndpointTests;

public sealed class QueryParametersBuilder
{
    private readonly Dictionary<string, string> _parameters = new()
    {
        { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
        { nameof(GetMetricDataRangeRequestParameters.MetricName), SpawnMetricsStorageTestsConstants.TestMetricName },
        { nameof(GetMetricDataRangeRequestParameters.RangeStart), DateTime.UtcNow.ToString("O") },
        { nameof(GetMetricDataRangeRequestParameters.RangeEnd), (DateTime.UtcNow + TimeSpan.FromHours(1)).ToString("O") }
    };

    public QueryParametersBuilder WithProjectName(string projectName)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.ProjectName)] = projectName;

        return this;
    }

    public QueryParametersBuilder WithMetricName(string metricName)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.MetricName)] = metricName;

        return this;
    }

    public QueryParametersBuilder WithRangeStart(DateTime rangeStart)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.RangeStart)] = rangeStart.ToString("O");

        return this;
    }

    public QueryParametersBuilder WithRangeEnd(DateTime rangeEnd)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.RangeEnd)] = rangeEnd.ToString("O");

        return this;
    }

    public Dictionary<string, string> Build()
    {
        return _parameters;
    }
}