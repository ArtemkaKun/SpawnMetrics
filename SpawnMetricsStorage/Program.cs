using SpawnMetricsStorage.Controllers;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

new MetricsController().RegisterEndpoints(app);

app.Run();