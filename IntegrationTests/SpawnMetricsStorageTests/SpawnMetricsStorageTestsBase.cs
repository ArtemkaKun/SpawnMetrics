using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Utils.SurrealDb;
using SurrealDb.Net;

namespace IntegrationTests.SpawnMetricsStorageTests;

public abstract class SpawnMetricsStorageTestsBase
{
    protected ISurrealDbClient SurrealDbClient = null!;

    private HttpClient _httpClient = null!;
    private string _testDatabaseName = null!;
    private string _validApiKey = null!;

    [OneTimeSetUp]
    public Task SetupEnvironment()
    {
        var configuration = BuildConfiguration();

        _testDatabaseName = configuration[SurrealDbConfigurationConstants.DatabaseConfigurationKey] ?? "TEST";
        _validApiKey = configuration[MetricsControllerConstants.ApiKeyParameter] ?? throw new ArgumentException($"{MetricsControllerConstants.ApiKeyParameter} is not set in configuration");

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

    protected Dictionary<string, string> CreateHeadersWithApiKey()
    {
        return new Dictionary<string, string>
        {
            { MetricsControllerConstants.ApiKeyParameter, _validApiKey }
        };
    }

    protected Task<HttpResponseMessage> PutAsync(string requestUri, Dictionary<string, string>? headers, object? content)
    {
        var jsonContent = JsonSerializer.Serialize(content);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri)
        {
            Content = stringContent
        };

        if (headers != null)
        {
            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }
        }

        return _httpClient.SendAsync(requestMessage);
    }

    protected Task<HttpResponseMessage> GetAsync(string requestUri, Dictionary<string, string>? queryParameters)
    {
        if (queryParameters != null)
        {
            QueryHelpers.AddQueryString(requestUri, queryParameters!);
        }
        
        return _httpClient.GetAsync(requestUri);
    }

    protected static Task DoRequestAndAssertBadRequest(Task<HttpResponseMessage> requestTask)
    {
        return DoRequestAndAssertHttpStatusCode(requestTask, HttpStatusCode.BadRequest);
    }

    protected static Task DoRequestAndAssertUnauthorized(Task<HttpResponseMessage> requestTask)
    {
        return DoRequestAndAssertHttpStatusCode(requestTask, HttpStatusCode.Unauthorized);
    }
    
    protected static Task DoRequestAndAssertOk(Task<HttpResponseMessage> requestTask)
    {
        return DoRequestAndAssertHttpStatusCode(requestTask, HttpStatusCode.OK);
    }

    private static async Task DoRequestAndAssertHttpStatusCode(Task<HttpResponseMessage> requestTask, HttpStatusCode expectedStatusCode)
    {
        var response = await requestTask;

        Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
    }
}