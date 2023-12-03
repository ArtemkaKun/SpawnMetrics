using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Services;
using SurrealDb.Net;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var surrealDbOptions = SurrealDbOptions
    .Create()
    .WithEndpoint(builder.Configuration["SURREAL_DB_ENDPOINT"])
    .WithNamespace(builder.Configuration["SURREAL_DB_NAMESPACE"])
    .WithDatabase(builder.Configuration["SURREAL_DB_DATABASE"])
    .WithUsername(builder.Configuration["SURREAL_DB_USER"])
    .WithPassword(builder.Configuration["SURREAL_DB_PASS"])
    .Build();

var surrealDbClient = new SurrealDbClient(surrealDbOptions);

var metricsService = new MetricsService(surrealDbClient);

new MetricsController(metricsService).RegisterEndpoints(app);

app.Run();