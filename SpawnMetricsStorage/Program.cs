using SpawnMetricsStorage.Controllers;
using SpawnMetricsStorage.Services;
using SpawnMetricsStorage.Utils.SurrealDb;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Services.AddCors(options => { options.AddPolicy("CorsPolicy", policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }); });
}

var app = builder.Build();

var surrealDbClient = SurrealDbCreator.CreateSurrealDbClient(builder.Configuration);
var metricsService = new MetricsService(surrealDbClient);
new MetricsController(metricsService, builder.Configuration).RegisterEndpoints(app);

if (builder.Environment.IsProduction())
{
    app.UseCors("CorsPolicy");
}

app.Run();