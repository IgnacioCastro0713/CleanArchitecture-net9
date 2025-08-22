using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OpenTelemetry.Logs;
using Web.Api;
using Web.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddCorsDefaultPolicy();

builder.Services.AddOpenTelemetry(builder.Configuration, serviceName: "web-api");
builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

WebApplication app = builder.Build();

RouteGroupBuilder groupBuilder = app.MapGroup("api");

app.MapEndpoints(groupBuilder);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Web.Api
{
    public partial class Program;
}
