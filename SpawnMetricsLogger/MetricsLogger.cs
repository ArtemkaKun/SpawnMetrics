using System.Text;
using System.Text.Json;
using LibGit2Sharp;
using SharedConstants;
using SpawnMetricsLogger.Config;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Models;

namespace SpawnMetricsLogger;

public class MetricsLogger
{
    private readonly ConfigModel _config;
    private readonly HttpClient _httpClient;

    public MetricsLogger(ConfigModel config, string adminApiKey)
    {
        _config = config;

        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(config.DataServerUrl);
        _httpClient.DefaultRequestHeaders.Add(MetricsControllerConstants.ApiKeyParameter, adminApiKey);
    }

    public void LogMetrics(Commit commit, string repoPath)
    {
        var metrics = MetricsCollector.CollectMetrics(commit, repoPath, _config);

        if (metrics == null)
        {
            return;
        }

        foreach (var metric in metrics)
        {
            var requestBody = new LogMetricRequestBody
            {
                ProjectName = _config.ProjectName,
                Metric = metric
            };

            var requestBodyJson = JsonSerializer.Serialize(requestBody);

            var response = _httpClient.PutAsync(EndpointsConstants.MetricEndpoint, new StringContent(requestBodyJson, Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode == false)
            {
                Console.WriteLine($"Failed to log metric {metric.Name}");
            }
            else
            {
                Console.WriteLine($"Logged metric {commit.Sha}");
            }
        }
    }
}