using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurrealDb.Net;

namespace IntegrationTests.SpawnMetricsStorageTests;

public abstract class SpawnMetricsStorageTestsBase
{
    private const string TestDatabaseName = "TEST_DATABASE";

    private HttpClient _httpClient = null!;
    private ISurrealDbClient _surrealDbClient = null!;

    [OneTimeSetUp]
    public virtual void SetupEnvironment()
    {
        var configuration = BuildConfiguration();

        var testServerAddress = configuration["MetricsStorageServerAddress"] ?? "http://localhost:5150";

        _httpClient = new HttpClient { BaseAddress = new Uri(testServerAddress) };

        var surrealDbOptions = SurrealDbOptions
            .Create()
            .WithEndpoint(configuration["SURREAL_DB_ENDPOINT"])
            .WithDatabase(TestDatabaseName)
            .WithUsername(configuration["SURREAL_DB_USER"])
            .WithPassword(configuration["SURREAL_DB_PASS"])
            .Build();

        _surrealDbClient = new SurrealDbClient(surrealDbOptions);

        CleanUpDatabase();
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
    public void CleanUpDatabase()
    {
        _surrealDbClient.Query($"REMOVE DATABASE {TestDatabaseName}");
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, object? content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        return await _httpClient.PostAsync(requestUri, stringContent);
    }
    
    public async Task DoRequestAndAssertBadRequest(Task<HttpResponseMessage> requestTask)
    {
        var response = await requestTask;

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }
}