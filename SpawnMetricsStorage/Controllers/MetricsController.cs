using Microsoft.AspNetCore.Mvc;
using SpawnMetricsStorage.Models;

namespace SpawnMetricsStorage.Controllers;

public sealed class MetricsController
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPut("/LogMetric", HandleLogMetricRequest);

        app.MapGet("/GetLatestMetricValue", HandleGetLatestMetricValue);

        app.MapGet("/GetMetricDataRange", HandleGetMetricDataRange);
    }

    private void HandleLogMetricRequest([FromBody] MetricRecord newMetricData)
    {
        throw new NotImplementedException();
    }

    private MetricRecord HandleGetLatestMetricValue([FromQuery] string metricName)
    {
        throw new NotImplementedException();
    }

    private List<MetricRecord> HandleGetMetricDataRange([FromQuery] string metricName, [FromQuery] DateTime rangeStart,
        [FromQuery] DateTime rangeEnd)
    {
        throw new NotImplementedException();
    }
}