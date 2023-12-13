using System.Text.Json;
using SharedConstants;
using SpawnMetricsStorage.Controllers;

namespace IntegrationTests.SpawnMetricsStorageTests.GetProjectNamesEndpointTests;

public sealed class GetProjectNamesTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public Task EmptyDatabaseReturnsEmptyList()
    {
        return RequestProjectNamesAndCheckResults([]);
    }

    [Test]
    public async Task DatabaseWithOneProjectReturnsOneProjectName()
    {
        await DefineTable(SpawnMetricsStorageTestsConstants.TestProjectName);

        await RequestProjectNamesAndCheckResults([SpawnMetricsStorageTestsConstants.TestProjectName]);
    }

    [Test]
    public async Task DatabaseWithMultipleProjectsReturnsMultipleProjectNames()
    {
        const string testProjectName2 = SpawnMetricsStorageTestsConstants.TestProjectName + "2";

        await DefineTable(SpawnMetricsStorageTestsConstants.TestProjectName);
        await DefineTable(SpawnMetricsStorageTestsConstants.AnotherProjectName);
        await DefineTable(testProjectName2);

        await RequestProjectNamesAndCheckResults([SpawnMetricsStorageTestsConstants.TestProjectName, SpawnMetricsStorageTestsConstants.AnotherProjectName, testProjectName2]);
    }

    private async Task DefineTable(string tableName)
    {
        await SurrealDbClient.Query($"DEFINE TABLE {tableName};");
    }

    private async Task RequestProjectNamesAndCheckResults(List<string> expectedNames)
    {
        var request = GetAsync(EndpointsConstants.ProjectNamesEndpoint, null);

        await DoRequestAndAssertOk(request);

        var response = await request;
        var responseContent = await response.Content.ReadAsStringAsync();
        var projectNames = JsonSerializer.Deserialize<List<string>>(responseContent);

        Assert.That(projectNames, Is.EquivalentTo(expectedNames));
    }
}