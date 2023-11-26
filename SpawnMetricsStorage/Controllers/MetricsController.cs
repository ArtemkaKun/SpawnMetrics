using Microsoft.AspNetCore.Mvc;
using SpawnMetricsStorage.Models;
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

    private async Task HandleLogMetricRequest([FromBody] MetricRecord newMetricData)
    {
        await _metricsService.WriteNewMetric(newMetricData);
    }

    private async Task<MetricRecord?> HandleGetLatestMetricByName([FromQuery] string metricName)
    {
        return await _metricsService.GetLatestMetricByName(metricName);
    }

    private async Task<List<MetricRecord>?> HandleGetMetricDataRange([FromQuery] string metricName, [FromQuery] DateTime rangeStart,
        [FromQuery] DateTime rangeEnd)
    {
        return await _metricsService.GetMetricDataRange(metricName, rangeStart, rangeEnd);
    }
}