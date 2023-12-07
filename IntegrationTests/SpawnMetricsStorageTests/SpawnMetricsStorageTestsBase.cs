using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SpawnMetricsStorage.Utils.SurrealDb;
using SurrealDb.Net;

namespace IntegrationTests.SpawnMetricsStorageTests;

public abstract class SpawnMetricsStorageTestsBase
{
    protected ISurrealDbClient SurrealDbClient = null!;
    
    private HttpClient _httpClient = null!;
    private string _testDatabaseName = null!;

    [OneTimeSetUp]
    public Task SetupEnvironment()
    {
        var configuration = BuildConfiguration();

        _testDatabaseName = configuration[SurrealDbConfigurationConstants.DatabaseConfigurationKey] ?? "TEST";

        var testServerAddress = configuration["MetricsStorageServerAddress"] ?? "http://localhost:5150";

        _httpClient = new HttpClient { BaseAddress = new Uri(testServerAddress) };

        SurrealDbClient = SurrealDbCreator.CreateSurrealDbClient(configuration);

        return CleanUpDatabase();
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("integration_test_configuration.json", false, true);

        var configuration = configurationBuilder.Build();

        return configuration;
    }

    [TearDown]
    public async Task CleanUpDatabase()
    {
        await SurrealDbClient.Query($"REMOVE DATABASE {_testDatabaseName}");
    }

    protected Task<HttpResponseMessage> PutAsync(string requestUri, object? content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        return _httpClient.PutAsync(requestUri, stringContent);
    }

    protected static async Task DoRequestAndAssertBadRequest(Task<HttpResponseMessage> requestTask)
    {
        var response = await requestTask;

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}