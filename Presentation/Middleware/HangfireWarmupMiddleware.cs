using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Presentation.Middleware;

/// <summary>
/// IIS'de uygulama başladığında Hangfire'ı tetikleyen middleware
/// </summary>
public class HangfireWarmupMiddleware
{
    private readonly RequestDelegate _next;
    private static bool _isWarmedUp = false;
    private static readonly object _lock = new();

    public HangfireWarmupMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<HangfireWarmupMiddleware> logger)
    {
        if (!_isWarmedUp)
        {
            lock (_lock)
            {
                if (!_isWarmedUp)
                {
                    try
                    {
                        logger.LogInformation("🔥 Hangfire warmup started...");
                        
                        // ✅ Hangfire server'ın çalıştığını kontrol et
                        using var connection = JobStorage.Current.GetConnection();
                        var servers = connection.GetAllItemsFromSet("servers");
                        
                        logger.LogInformation("✅ Hangfire warmup completed. Active servers: {Count}", servers.Count);
                        
                        _isWarmedUp = true;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "❌ Hangfire warmup failed");
                    }
                }
            }
        }

        await _next(context);
    }
}

public static class HangfireWarmupMiddlewareExtensions
{
    public static IApplicationBuilder UseHangfireWarmup(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HangfireWarmupMiddleware>();
    }
}