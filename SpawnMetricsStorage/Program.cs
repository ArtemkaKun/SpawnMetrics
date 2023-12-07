using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Services;
using SpawnMetricsStorage.Utils.SurrealDb;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var surrealDbClient = SurrealDbCreator.CreateSurrealDbClient(builder.Configuration);
var metricsService = new MetricsService(surrealDbClient);
new MetricsController(metricsService, builder.Configuration).RegisterEndpoints(app);

app.Run();