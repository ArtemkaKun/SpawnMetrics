using SurrealDb.Net;

namespace SpawnMetricsStorage.Utils.SurrealDb;

public static class SurrealDbCreator
{
    public static SurrealDbClient CreateSurrealDbClient(IConfiguration configuration)
    {
        var surrealDbOptions = SurrealDbOptions
            .Create()
            .WithEndpoint(configuration[SurrealDbConfigurationConstants.EndpointConfigurationKey])
            .WithNamespace(configuration[SurrealDbConfigurationConstants.NamespaceConfigurationKey])
            .WithDatabase(configuration[SurrealDbConfigurationConstants.DatabaseConfigurationKey])
            .WithUsername(configuration[SurrealDbConfigurationConstants.UsernameConfigurationKey])
            .WithPassword(configuration[SurrealDbConfigurationConstants.PasswordConfigurationKey])
            .Build();

        return new SurrealDbClient(surrealDbOptions);
    }
}