using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Services;

namespace SpawnMetricsStorage.Controllers;

public sealed class MetricsController(MetricsService metricsService)
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPut(MetricsControllerConstants.LogMetricEndpoint, HandleLogMetricRequest);

        app.MapGet("/GetLatestMetric", HandleGetLatestMetricByName);

        app.MapGet("/GetProjectNames", HandleGetProjectNames);

        app.MapGet("/GetMetricDataRange", HandleGetMetricDataRange);
    }

    private async Task HandleLogMetricRequest([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] LogMetricRequestBody newMetricData)
    {
        await metricsService.WriteNewMetric(newMetricData);
    }

    private async Task<MetricRecord?> HandleGetLatestMetricByName([FromQuery] GetMetricRequestParameters parameters)
    {
        return await metricsService.GetLatestMetricByName(parameters.ProjectName, parameters.MetricName);
    }

    private async Task<List<string>?> HandleGetProjectNames()
    {
        return await metricsService.GetProjectNames();
    }

    private async Task<List<MetricRecord>?> HandleGetMetricDataRange([FromQuery] GetMetricDataRangeRequestParameters parameters)
    {
        return await metricsService.GetMetricDataRange(parameters.ProjectName, parameters.MetricName, parameters.RangeStart, parameters.RangeEnd);
    }
}