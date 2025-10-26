using Serilog;

namespace Presentation.Extensions;

public static class LoggerExtension
{
    public static void AddSerilogExtension(ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();
        logging.AddSerilog();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration) // Reads settings from appsettings.json
            //.Enrich.FromLogContext()
            .WriteTo.Sentry(o => o.Dsn = "https://59db11e110de38f9a5e1bc3a4d638bca@o4508148841709568.ingest.de.sentry.io/4509019195834448")
            .CreateLogger();

        try
        {
            var message = configuration.GetValue<string>("Control");
            Log.Information(message!);
            Log.Information("Starting up the application");
            Log.Warning("Test Warning");
            Log.Fatal("Test Fatal");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
            throw;
        }
        
        // Ensure logs are flushed on application shutdown
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

    }
}