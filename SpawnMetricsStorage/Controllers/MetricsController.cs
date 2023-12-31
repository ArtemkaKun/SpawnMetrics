using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedConstants;
using SpawnMetricsStorage.Models;
using SpawnMetricsStorage.Services;
using SpawnMetricsStorage.Utils;

namespace SpawnMetricsStorage.Controllers;

public sealed class MetricsController(MetricsService metricsService, IConfiguration configuration)
{
    private readonly string _validApiKey = configuration[MetricsControllerConstants.ApiKeyParameter] ?? throw new ArgumentException(MetricsControllerConstants.ApiKeyNotDefinedError);

    public void RegisterEndpoints(WebApplication app)
    {
        app.MapPut(EndpointsConstants.MetricEndpoint, HandleLogMetricRequest);

        app.MapGet(EndpointsConstants.ProjectNamesEndpoint, HandleGetProjectNames);

        app.MapGet(EndpointsConstants.GetAllMetricsEndpoint, HandleGetAllMetrics);
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

    private async Task<IResult> HandleGetAllMetrics([AsParameters] GetAllMetricsRequestParameters parameters)
    {
        var validationError = ModelValidator.Validate(parameters);

        if (validationError != null)
        {
            return validationError;
        }

        var metrics = await metricsService.GetAllMetrics(parameters.ProjectName);

        return Results.Ok(metrics);
    }
}