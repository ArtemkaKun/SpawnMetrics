using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurrealDb.Net;

namespace IntegrationTests.SpawnMetricsStorageTests;

public abstract class SpawnMetricsStorageTestsBase
{
    private const string TestDatabaseName = "TEST";

    protected ISurrealDbClient SurrealDbClient = null!;

    private HttpClient _httpClient = null!;

    [OneTimeSetUp]
    public Task SetupEnvironment()
    {
        var configuration = BuildConfiguration();

        var testServerAddress = configuration["MetricsStorageServerAddress"] ?? "http://localhost:5150";

        _httpClient = new HttpClient { BaseAddress = new Uri(testServerAddress) };

        var surrealDbOptions = SurrealDbOptions
            .Create()
            .WithEndpoint(configuration["SURREAL_DB_ENDPOINT"])
            .WithNamespace(configuration["SURREAL_DB_NAMESPACE"])
            .WithDatabase(TestDatabaseName)
            .WithUsername(configuration["SURREAL_DB_USER"])
            .WithPassword(configuration["SURREAL_DB_PASS"])
            .Build();

        SurrealDbClient = new SurrealDbClient(surrealDbOptions);

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
        await SurrealDbClient.Query($"REMOVE DATABASE {TestDatabaseName}");
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