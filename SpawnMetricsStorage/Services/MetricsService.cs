using System.Text.Json;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SurrealDb.Net;

namespace SpawnMetricsStorage.Services;

public sealed class MetricsService(ISurrealDbClient surrealDbClient)
{
    public async Task WriteNewMetric(LogMetricRequestBody newMetricData)
    {
        await surrealDbClient.Create(newMetricData.ProjectName, newMetricData.Metric);
    }

    public async Task<MetricRecord?> GetLatestMetricByName(string projectName, string metricName)
    {
        var queryResponse = await surrealDbClient.Query($"SELECT * FROM {projectName} WHERE Name = \"{metricName}\" ORDER BY LogTime DESC LIMIT 1;");

        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        return list?.FirstOrDefault();
    }

    public async Task<List<string>?> GetProjectNames()
    {
        var queryResponse = await surrealDbClient.Query("INFO FOR DATABASE;");
        var databaseMetadata = queryResponse.GetValue<JsonElement>(0);

        return GetProjectNamesFromDatabaseMetadata(databaseMetadata);
    }

    private static List<string>? GetProjectNamesFromDatabaseMetadata(JsonElement databaseMetadata)
    {
        var tablesList = databaseMetadata.GetProperty("tables");
        var tableNameToMetadataMap = tablesList.Deserialize<Dictionary<string, string>>();

        return tableNameToMetadataMap?.Keys.ToList();
    }

    public async Task<List<MetricRecord>?> GetMetricDataRange(string projectName, string metricName, DateTime rangeStart, DateTime rangeEnd)
    {
        var queryResponse = await surrealDbClient.Query($"SELECT * FROM {projectName} WHERE (Name = \"{metricName}\" AND LogTime >= \"{rangeStart}\" AND LogTime <= \"{rangeEnd}\") ORDER BY LogTime DESC;");

        return queryResponse.GetValue<List<MetricRecord>>(0);
    }
}