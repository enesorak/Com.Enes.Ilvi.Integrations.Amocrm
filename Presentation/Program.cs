using Application;
using Application.Amocrm;
using Hangfire;
using Infrastructure;
using Presentation.Endpoints;
using Presentation.Extensions;
using Presentation.Middleware;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Serilog configuration
LoggerExtension.AddSerilogExtension(builder.Logging, builder.Configuration);
builder.Host.UseSerilog();

// ✅ Sentry configuration
builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://59db11e110de38f9a5e1bc3a4d638bca@o4508148841709568.ingest.de.sentry.io/4509019195834448";
    options.Debug = false; // Production'da false olmalı
    options.TracesSampleRate = 1.0;
    options.DiagnosticLevel = SentryLevel.Warning; // Debug yerine Warning
});

// ✅ Application & Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ✅ Sentry lifecycle
app.Lifetime.ApplicationStopped.Register(SentrySdk.Close);

// ✅ Middleware pipeline
app.UseSentryTracing();
app.UseRouting();

// ✅ CRITICAL: Hangfire warmup - IIS başladığında Hangfire'ı başlat
app.UseHangfireWarmup();

// ✅ Database migration
app.ApplyMigrations();

// ✅ OpenAPI & Swagger
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.MapControllers();

// ✅ Hangfire Dashboard - Authentication eklemek isterseniz sonra yaparız
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "AmoCRM Integration - Background Jobs",
    StatsPollingInterval = 5000, // 5 saniye
    DisplayStorageConnectionString = false
});

// ✅ Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
    };
});

// ==========================================
// ✅ ENDPOINTS
// ==========================================

// Configuration endpoints
app.MapConfigurationEndpoints();

// AmoCRM entity endpoints
app.MapUsersEndpoint();
app.MapContactsEndpoint();
app.MapPipelinesEndpoint();
app.MapLeadsEndpoint();
app.MapTasksEndpoint();
app.MapEventsEndpoint();

// ✅ NEW: Monitoring endpoints
app.MapMonitoringEndpoints();

// ==========================================
// ✅ UTILITY ENDPOINTS
// ==========================================

app.MapGet("alive", () => Results.Ok(new
{
    Status = "Online",
    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
    Timestamp = DateTime.UtcNow,
    MachineName = Environment.MachineName
}))
.WithName("Alive Check")
.WithTags("Health");

app.MapGet("get-configuration", (IConfiguration configuration) => 
    Results.Ok(new
    {
        BaseUrl = configuration.GetValue<string>("Options:Base"),
        Database = configuration.GetValue<string>("Options:Db"),
        Company = configuration.GetValue<string>("Company"),
        Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
    }))
.WithName("Get Configuration")
.WithTags("Configuration");

app.MapPost("api/amocrm/test-uri", async (
    string url, 
    IServiceHandler handler, 
    ILogger<string> logger) =>
{
    logger.LogWarning("Testing URL: {Url}", url);
    try
    {
        var result = await handler.UrlTesting(url, CancellationToken.None);
        return Results.Ok(new { Success = true, Data = result });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "URL test failed");
        return Results.Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "URL Test Failed"
        );
    }
})
.WithName("Test AmoCRM URL")
.WithTags("Testing");

app.MapGet("logger-test", (ILogger<string> logger) =>
{
    logger.LogInformation("Logger test - Information");
    logger.LogWarning("Logger test - Warning");
    logger.LogError("Logger test - Error");
    return Results.Ok("Logger tested - check logs");
})
.WithName("Logger Test")
.WithTags("Testing");

app.MapGet("sentry-test", () =>
{
    SentrySdk.CaptureMessage("Sentry test message from Program.cs");
    return Results.Ok("Sentry message sent - check Sentry dashboard");
})
.WithName("Sentry Test")
.WithTags("Testing");

// ==========================================
// ✅ STARTUP LOGGING
// ==========================================

Log.Information("========================================");
Log.Information("🚀 AmoCRM Integration Service Started");
Log.Information("========================================");
Log.Information("Environment: {Environment}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
Log.Information("Company: {Company}", builder.Configuration.GetValue<string>("Company"));
Log.Information("Database: {Database}", builder.Configuration.GetValue<string>("Options:Db"));
Log.Information("Machine: {Machine}", Environment.MachineName);
Log.Information("========================================");

// ✅ Sentry startup message
SentrySdk.CaptureMessage($"AmoCRM Integration started - {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

app.Run();