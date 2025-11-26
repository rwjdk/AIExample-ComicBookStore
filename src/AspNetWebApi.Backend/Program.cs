using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedBackend;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

BackendConfiguration backendConfiguration = builder.Configuration.GetBackendConfiguration();

builder.Services.AddAGUI();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors",
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapAGUI("/comic-book-guy", AgentBuilder.GetComicBookGuy(backendConfiguration));
app.MapAGUI("/assistant", AgentBuilder.GetAssistant(backendConfiguration));

app.UseCors("Cors");

app.UseHttpsRedirection();

app.Run();