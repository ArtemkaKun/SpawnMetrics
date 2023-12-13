using SpawnMetricsStorage.Models;

namespace IntegrationTests.SpawnMetricsStorageTests.GetAllMetricsEndpointTests;

public sealed class QueryParametersBuilder
{
    private readonly Dictionary<string, string> _parameters = new()
    {
        { nameof(GetAllMetricsRequestParameters.ProjectName), SpawnMetricsStorageTestsConstants.TestProjectName },
    };

    public QueryParametersBuilder WithProjectName(string projectName)
    {
        _parameters[nameof(GetAllMetricsRequestParameters.ProjectName)] = projectName;

        return this;
    }

    public QueryParametersBuilder WithoutProjectName()
    {
        _parameters.Remove(nameof(GetAllMetricsRequestParameters.ProjectName));

        return this;
    }

    public Dictionary<string, string> Build()
    {
        return _parameters;
    }
}