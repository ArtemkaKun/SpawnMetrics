using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Models.MetricRecordFiles;
using SpawnMetricsStorage.Services;
using SpawnMetricsStorage.Utils;

namespace SpawnMetricsStorage.Controllers;

public sealed class MetricsController(MetricsService metricsService, IConfiguration configuration)
{
    private readonly string _validApiKey = configuration[MetricsControllerConstants.ApiKeyParameter] ?? throw new ArgumentException(MetricsControllerConstants.ApiKeyNotDefinedError);

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPut(MetricsControllerConstants.MetricEndpoint, HandleLogMetricRequest);

        app.MapGet(MetricsControllerConstants.LatestMetricEndpoint, HandleGetLatestMetricByName);

        app.MapGet(MetricsControllerConstants.ProjectNamesEndpoint, HandleGetProjectNames);

        app.MapGet(MetricsControllerConstants.MetricDataRangeEndpoint, HandleGetMetricDataRange);
    }

    private async Task<IResult> HandleLogMetricRequest([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] LogMetricRequestBody newMetricData, HttpContext context)
    {
        if (IsAuthorizedWithApiKey(context) == false)
        {
            return Results.Unauthorized();
        }

        var validationError = ModelValidator.Validate(newMetricData);

        if (validationError != null)
        {
            return validationError;
        }

        await metricsService.WriteNewMetric(newMetricData);

        return Results.Ok();
    }

    private bool IsAuthorizedWithApiKey(HttpContext context)
    {
        var apiKey = context.Request.Headers[MetricsControllerConstants.ApiKeyParameter];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return false;
        }

        return apiKey == _validApiKey;
    }

    private async Task<MetricRecord?> HandleGetLatestMetricByName([AsParameters] GetMetricRequestParameters parameters)
    {
        return await metricsService.GetLatestMetricByName(parameters.ProjectName, parameters.MetricName);
    }

    private Task<List<string>?> HandleGetProjectNames()
    {
        return metricsService.GetProjectNames();
    }

    private async Task<List<MetricRecord>?> HandleGetMetricDataRange([AsParameters] GetMetricDataRangeRequestParameters parameters)
    {
        return await metricsService.GetMetricDataRange(parameters.ProjectName, parameters.MetricName,
            parameters.RangeStart, parameters.RangeEnd);
    }
}