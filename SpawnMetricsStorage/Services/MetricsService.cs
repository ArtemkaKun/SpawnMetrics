using SpawnMetricsStorage.Models;
using SurrealDb.Net;

namespace SpawnMetricsStorage.Services;

public sealed class MetricsService
{
    private const string MetricsTableName = "Metrics";

    private readonly ISurrealDbClient _surrealDbClient;

    public MetricsService(ISurrealDbClient surrealDbClient)
    {
        _surrealDbClient = surrealDbClient;
    }

    public async Task WriteNewMetric(MetricRecord newMetricData)
    {
        await _surrealDbClient.Create(MetricsTableName, newMetricData);
    }

    public async Task<MetricRecord?> GetLatestMetricByName(string metricName)
    {
        var queryResponse =
            await _surrealDbClient.Query(
                $"SELECT * FROM {MetricsTableName} WHERE Name = {metricName} ORDER BY LogTime DESC LIMIT 1;");

        if (queryResponse.FirstResult == null || queryResponse.FirstResult.IsError)
        {
            return null;
        }

        return queryResponse.GetValue<MetricRecord>(0);
    }

    public async Task<List<MetricRecord>?> GetMetricDataRange(string metricName, DateTime rangeStart, DateTime rangeEnd)
    {
        var queryResponse =
            await _surrealDbClient.Query(
                $"SELECT * FROM {MetricsTableName} WHERE Name = {metricName} AND LogTime >= {rangeStart} AND LogTime <= {rangeEnd} ORDER BY LogTime DESC;");

        if (queryResponse.FirstResult == null || queryResponse.FirstResult.IsError)
        {
            return null;
        }

        var data = new List<MetricRecord>(queryResponse.Count);

        for (var recordIndex = 0; recordIndex < queryResponse.Count; recordIndex++)
        {
            var queryValue = queryResponse.GetValue<MetricRecord>(recordIndex);

            if (queryValue != null)
            {
                data.Add(queryValue);
            }
        }

        return data;
    }
}