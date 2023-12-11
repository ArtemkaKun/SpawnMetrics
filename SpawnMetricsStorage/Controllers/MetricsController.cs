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

    private Task<List<string>?> HandleGetProjectNames()
    {
        return metricsService.GetProjectNames();
    }

    private async Task<IResult> HandleGetMetricDataRange([AsParameters] GetMetricDataRangeRequestParameters parameters)
    {
        var validationError = ModelValidator.Validate(parameters);

        if (validationError != null)
        {
            return validationError;
        }

        var metrics = await metricsService.GetMetricDataRange(parameters.ProjectName, parameters.MetricName, parameters.RangeStart, parameters.RangeEnd);

        return Results.Ok(metrics);
    }
}