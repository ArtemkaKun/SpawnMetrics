using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Services;
using SpawnMetricsStorage.Utils;

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


    private async Task<IResult> HandleLogMetricRequest([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] LogMetricRequestBody newMetricData)
    {
        var validationError = ModelValidator.Validate(newMetricData);

        if (validationError != null)
        {
            return validationError;
        }

        await metricsService.WriteNewMetric(newMetricData);

        return Results.Ok();
    }

    private async Task<MetricRecord?> HandleGetLatestMetricByName([AsParameters] GetMetricRequestParameters parameters)
    {
        return await metricsService.GetLatestMetricByName(parameters.ProjectName, parameters.MetricName);
    }

    private async Task<List<string>?> HandleGetProjectNames()
    {
        return await metricsService.GetProjectNames();
    }

    private async Task<List<MetricRecord>?> HandleGetMetricDataRange([AsParameters] GetMetricDataRangeRequestParameters parameters)
    {
        return await metricsService.GetMetricDataRange(parameters.ProjectName, parameters.MetricName,
            parameters.RangeStart, parameters.RangeEnd);
    }
}