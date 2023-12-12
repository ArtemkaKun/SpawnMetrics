using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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
        var response = await _httpClient.GetAsync("/ProjectNames");
        var responseContent = response.Content.ReadAsStringAsync().Result;
        
        return JsonSerializer.Deserialize<List<string>>(responseContent);
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