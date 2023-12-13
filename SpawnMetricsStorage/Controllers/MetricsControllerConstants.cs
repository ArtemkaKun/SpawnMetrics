namespace SpawnMetricsStorage.Controllers;

public static class MetricsControllerConstants
{
    public const string MetricEndpoint = "/Metric";
    public const string ProjectNamesEndpoint = "/ProjectNames";
    public const string GetAllMetricsEndpoint = "/AllMetrics";
    
    public const string ApiKeyParameter = "API_KEY";
    public const string ApiKeyNotDefinedError = ApiKeyParameter + " is not set in configuration";
}