using System.Text.Json;
using SpawnMetricsStorage.Controllers;

namespace IntegrationTests.SpawnMetricsStorageTests.GetProjectNamesEndpointTests;

public class GetProjectNamesTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public Task EmptyDatabaseReturnsEmptyList()
    {
        return RequestProjectNamesAndCheckResults([]);
    }

    [Test]
    public async Task DatabaseWithOneProjectReturnsOneProjectName()
    {
        await DefineTable("TEST");

        await RequestProjectNamesAndCheckResults(["TEST"]);
    }

    [Test]
    public async Task DatabaseWithMultipleProjectsReturnsMultipleProjectNames()
    {
        await DefineTable("TEST");
        await DefineTable("TEST1");
        await DefineTable("TEST2");

        await RequestProjectNamesAndCheckResults(["TEST", "TEST1", "TEST2"]);
    }

    private async Task DefineTable(string tableName)
    {
        await SurrealDbClient.Query($"DEFINE TABLE {tableName};");
    }

    private async Task RequestProjectNamesAndCheckResults(List<string> expectedNames)
    {
        var request = GetAsync(MetricsControllerConstants.ProjectNamesEndpoint, null);

        await DoRequestAndAssertOk(request);

        var response = await request;
        var responseContent = await response.Content.ReadAsStringAsync();
        var projectNames = JsonSerializer.Deserialize<List<string>>(responseContent);

        Assert.That(projectNames, Is.EqualTo(expectedNames));
    }
}