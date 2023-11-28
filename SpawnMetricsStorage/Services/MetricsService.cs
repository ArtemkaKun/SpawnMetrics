using SpawnMetricsStorage.Models;
using SurrealDb.Net;

namespace SpawnMetricsStorage.Services;

public sealed class MetricsService
{
    private readonly ISurrealDbClient _surrealDbClient;

    public MetricsService(ISurrealDbClient surrealDbClient)
    {
        _surrealDbClient = surrealDbClient;
    }

    public async Task WriteNewMetric(LogMetricRequestBody newMetricData)
    {
        await _surrealDbClient.Create(newMetricData.ProjectName, newMetricData);
    }

    public async Task<MetricRecord?> GetLatestMetricByName(string projectName, string metricName)
    {
        var queryResponse =
            await _surrealDbClient.Query(
                $"SELECT * FROM {projectName} WHERE Name = \"{metricName}\" ORDER BY LogTime DESC LIMIT 1;");

        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        return list?.FirstOrDefault();
    }

    public async Task<List<MetricRecord>?> GetMetricDataRange(string projectName, string metricName, DateTime rangeStart, DateTime rangeEnd)
    {
        var queryResponse =
            await _surrealDbClient.Query(
                $"SELECT * FROM {projectName} WHERE (Name = \"{metricName}\" AND LogTime >= \"{rangeStart}\" AND LogTime <= \"{rangeEnd}\") ORDER BY LogTime DESC;");

        return queryResponse.GetValue<List<MetricRecord>>(0);
    }
}