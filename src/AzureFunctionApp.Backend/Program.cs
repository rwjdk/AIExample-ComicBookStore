using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedBackend;

FunctionsApplicationBuilder builder = FunctionsApplication.CreateBuilder(args);

//Configuration
BackendConfiguration backendConfiguration = builder.Configuration.GetBackendConfiguration();

builder.Services.AddSingleton(backendConfiguration);

builder.AddServiceDefaults();

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();