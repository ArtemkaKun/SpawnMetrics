using SpawnMetricsStorage.Models;

namespace IntegrationTests.SpawnMetricsStorageTests.GetMetricDataRangeEndpointTests;

public sealed class QueryParametersBuilder
{
    private readonly Dictionary<string, string> _parameters = new()
    {
        { nameof(GetMetricDataRangeRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
        { nameof(GetMetricDataRangeRequestParameters.RangeStart), DateTimeToString(DateTime.UtcNow) },
        { nameof(GetMetricDataRangeRequestParameters.RangeEnd), DateTimeToString(DateTime.UtcNow + TimeSpan.FromHours(1)) }
    };

    public QueryParametersBuilder WithProjectName(string projectName)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.ProjectName)] = projectName;

        return this;
    }

    public QueryParametersBuilder WithoutProjectName()
    {
        _parameters.Remove(nameof(GetMetricDataRangeRequestParameters.ProjectName));

        return this;
    }

    public QueryParametersBuilder WithRangeStart(DateTime rangeStart)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.RangeStart)] = DateTimeToString(rangeStart);

        return this;
    }

    public QueryParametersBuilder WithoutRangeStart()
    {
        _parameters.Remove(nameof(GetMetricDataRangeRequestParameters.RangeStart));

        return this;
    }

    public QueryParametersBuilder WithRangeEnd(DateTime rangeEnd)
    {
        _parameters[nameof(GetMetricDataRangeRequestParameters.RangeEnd)] = DateTimeToString(rangeEnd);

        return this;
    }

    public QueryParametersBuilder WithoutRangeEnd()
    {
        _parameters.Remove(nameof(GetMetricDataRangeRequestParameters.RangeEnd));

        return this;
    }

    public Dictionary<string, string> Build()
    {
        return _parameters;
    }

    private static string DateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString("O");
    }
}