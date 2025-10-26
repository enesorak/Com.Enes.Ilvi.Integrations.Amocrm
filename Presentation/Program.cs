using Application;
using Application.Amocrm;
using Hangfire;
using Infrastructure;
using Presentation.Endpoints;
using Presentation.Extensions;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
LoggerExtension.AddSerilogExtension(builder.Logging, builder.Configuration);

builder.Host.UseSerilog();
 

builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://59db11e110de38f9a5e1bc3a4d638bca@o4508148841709568.ingest.de.sentry.io/4509019195834448";
    options.Debug = true; // Enable debug logging for initial setup 
    options.TracesSampleRate = 1.0;
    options.DiagnosticLevel = SentryLevel.Debug;
});
 



builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();

app.Lifetime.ApplicationStopped.Register(SentrySdk.Close);
 
 
 

app.UseSentryTracing();
app.UseRouting();

app.ApplyMigrations();

app.MapOpenApi();
app.UseHttpsRedirection();

app.MapControllers();
app.MapScalarApiReference( );
app.UseHangfireDashboard();
app.MapConfigurationEndpoints();



app.MapUsersEndpoint();
app.MapContactsEndpoint();
app.MapPipelinesEndpoint();
app.MapLeadsEndpoint();

app.MapTasksEndpoint();
app.MapEventsEndpoint();
 
app.MapGet("get-configuration",
    (IConfiguration configuration) => Results.Ok(configuration.GetValue<string>("Options:Base")));


app.MapPost("api/amocrm/test-uri", async (string url, IServiceHandler handler, ILogger<string> logger) =>
{
    logger.LogWarning("Url Testing");
    var result = await handler.UrlTesting(url, CancellationToken.None);
    return Results.Ok(result);
});

app.MapGet("alive", () => Results.Ok("Online"));


app.MapGet("logger-test", (ILogger<string> logger) => logger.LogWarning("Logger Testing") ); 
 
app.MapGet("sentry-test", () =>
{
    SentrySdk.CaptureMessage("Sentry is working!");
    return Results.Ok("Sentry logged this message.");
});
app.UseSerilogRequestLogging();
SentrySdk.CaptureMessage("Hello Sentry");
app.Run();

 