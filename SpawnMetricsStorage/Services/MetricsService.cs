using System.Text.Json;
using Humanizer;
using MetricRecordModel;
using SpawnMetricsStorage.Models;
using SurrealDb.Net;

namespace SpawnMetricsStorage.Services;

public sealed class MetricsService(ISurrealDbClient surrealDbClient)
{
    private readonly string _metricRecordLogTimeUtcDbFriendly = nameof(MetricRecord.LogTimeUtc).Underscore();

    public async Task WriteNewMetric(LogMetricRequestBody newMetricData)
    {
        await surrealDbClient.Create(newMetricData.ProjectName, newMetricData.Metric);
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

    public async Task<List<MetricRecord>?> GetMetricDataRange(string projectName, DateTime rangeStart, DateTime rangeEnd)
    {
        var query =
            $"SELECT * FROM {projectName} WHERE ({_metricRecordLogTimeUtcDbFriendly} >= \"{rangeStart:O}\" AND {_metricRecordLogTimeUtcDbFriendly} <= \"{rangeEnd:O}\") ORDER BY {_metricRecordLogTimeUtcDbFriendly} DESC;";

        var queryResponse = await surrealDbClient.Query(query);

        var list = queryResponse.GetValue<List<MetricRecord>>(0);

        if (list?.Count == 0)
        {
            return null;
        }

        return list;
    }
}