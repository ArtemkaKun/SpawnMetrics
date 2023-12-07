using System.Text.Json;
using SpawnMetricsStorage.Controllers;

namespace IntegrationTests.SpawnMetricsStorageTests.GetProjectNamesEndpointTests;

public class GetProjectNamesTests : SpawnMetricsStorageTestsBase
{
    [Test]
    public async Task EmptyDatabaseReturnsEmptyList()
    {
        var request = GetAsync(MetricsControllerConstants.ProjectNamesEndpoint, null);

        await DoRequestAndAssertOk(request);

        var response = await request;
        var responseContent = await response.Content.ReadAsStringAsync();
        var projectNames = JsonSerializer.Deserialize<List<string>>(responseContent);

        Assert.That(projectNames, Is.Null.Or.Empty);
    }

    [Test]
    public async Task DatabaseWithOneProjectReturnsOneProjectName()
    {
        await SurrealDbClient.Query("DEFINE TABLE TEST;");

        var request = GetAsync(MetricsControllerConstants.ProjectNamesEndpoint, null);

        await DoRequestAndAssertOk(request);

        var response = await request;

        var responseContent = await response.Content.ReadAsStringAsync();

        var projectNames = JsonSerializer.Deserialize<List<string>>(responseContent);

        Assert.That(projectNames, Is.Not.Null.And.Not.Empty);
        Assert.That(projectNames!.Count, Is.EqualTo(1));
        Assert.That(projectNames[0], Is.EqualTo("TEST"));
    }

    [Test]
    public async Task DatabaseWithMultipleProjectsReturnsMultipleProjectNames()
    {
        await SurrealDbClient.Query("DEFINE TABLE TEST;");
        await SurrealDbClient.Query("DEFINE TABLE TEST2;");
        await SurrealDbClient.Query("DEFINE TABLE TEST3;");

        var request = GetAsync(MetricsControllerConstants.ProjectNamesEndpoint, null);

        await DoRequestAndAssertOk(request);

        var response = await request;

        var responseContent = await response.Content.ReadAsStringAsync();

        var projectNames = JsonSerializer.Deserialize<List<string>>(responseContent);

        Assert.That(projectNames, Is.Not.Null.And.Not.Empty);
        Assert.That(projectNames!.Count, Is.EqualTo(3));
        Assert.That(projectNames[0], Is.EqualTo("TEST"));
        Assert.That(projectNames[1], Is.EqualTo("TEST2"));
        Assert.That(projectNames[2], Is.EqualTo("TEST3"));
    }
}