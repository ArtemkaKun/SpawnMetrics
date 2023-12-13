using System.Text.Json;
using MetricRecordModel;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharedConstants;

namespace SpawnMetricsView.DataCollectorFiles;

public sealed class DataCollector
{
    private readonly HttpClient _httpClient;

    public DataCollector()
    {
        var configuration = BuildConfiguration();
        var testServerAddress = configuration["MetricsStorageServerAddress"] ?? throw new ArgumentException("MetricsStorageServerAddress not defined");
        _httpClient = new HttpClient { BaseAddress = new Uri(testServerAddress) };
    }

    public async Task<List<string>?> GetProjectNames()
    {
        var response = await _httpClient.GetAsync(EndpointsConstants.ProjectNamesEndpoint);
        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<string>?>(responseContent);
    }

    public async Task<List<MetricRecord>?> GetMetricDataRange(string projectName)
    {
        var response = await _httpClient.GetAsync($"{EndpointsConstants.GetAllMetricsEndpoint}?ProjectName={projectName}");
        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<MetricRecord>?>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var configurationBuilder = new WebAssemblyHostConfiguration();

        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("appsettings.json", false, true);

        var configuration = configurationBuilder.Build();

        return configuration;
    }
}