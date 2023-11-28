using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Services;

namespace SpawnMetricsStorage.Controllers;

public sealed class MetricsController
{
    private readonly MetricsService _metricsService;

    public MetricsController(MetricsService metricsService)
    {
        _metricsService = metricsService;
    }

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPut("/LogMetric", HandleLogMetricRequest);

        app.MapGet("/GetLatestMetric", HandleGetLatestMetricByName);

        app.MapGet("/GetMetricDataRange", HandleGetMetricDataRange);
    }

    private async Task HandleLogMetricRequest([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] LogMetricRequestBody newMetricData)
    {
        await _metricsService.WriteNewMetric(newMetricData);
    }

    private async Task<MetricRecord?> HandleGetLatestMetricByName([FromQuery] string projectName, [FromQuery] string metricName)
    {
        return await _metricsService.GetLatestMetricByName(projectName, metricName);
    }

    private async Task<List<MetricRecord>?> HandleGetMetricDataRange([FromQuery] string projectName, [FromQuery] string metricName, [FromQuery] DateTime rangeStart,
        [FromQuery] DateTime rangeEnd)
    {
        return await _metricsService.GetMetricDataRange(projectName, metricName, rangeStart, rangeEnd);
    }
}