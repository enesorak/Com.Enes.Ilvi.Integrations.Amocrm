using Hangfire;
using Hangfire.Storage;
using Infrastructure;

namespace Presentation.Endpoints;

public static class MonitoringEndpoints
{
    public static void MapMonitoringEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // ✅ Hangfire sağlık kontrolü
        endpoints.MapGet("api/health/hangfire", () =>
        {
            try
            {
                using var connection = JobStorage.Current.GetConnection();
                var servers = connection.GetAllItemsFromSet("servers");
                var stats = connection.GetAllItemsFromSet("recurring-jobs");

                return Results.Ok(new
                {
                    Status = "Healthy",
                    ActiveServers = servers.Count,
                    RecurringJobs = stats.Count,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 503,
                    title: "Hangfire Unhealthy"
                );
            }
        }).WithName("Hangfire Health Check");

        // ✅ Database connection check
        
        endpoints.MapGet("api/health/database", async (ApplicationDbContext dbContext) =>
        {
            try
            {
                var canConnect = await dbContext.Database.CanConnectAsync();
                return Results.Ok(new
                {
                    Status = canConnect ? "Healthy" : "Unhealthy",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 503,
                    title: "Database Unhealthy"
                );
            }
        }).WithName("Database Health Check");

        // ✅ Son çalışan job'ları listele
        endpoints.MapGet("api/monitoring/recent-jobs", () =>
        {
            try
            {
                var monitoringApi = JobStorage.Current.GetMonitoringApi();
                var succeededJobs = monitoringApi.SucceededJobs(0, 10);
                var failedJobs = monitoringApi.FailedJobs(0, 10);
                var processingJobs = monitoringApi.ProcessingJobs(0, 10);

                return Results.Ok(new
                {
                    Succeeded = succeededJobs.Select(j => new
                    {
                        j.Key,
                        j.Value.Job.Method.Name,
                        j.Value.SucceededAt
                    }),
                    Failed = failedJobs.Select(j => new
                    {
                        j.Key,
                        j.Value.Job.Method.Name,
                        j.Value.FailedAt,
                        Reason = j.Value.ExceptionMessage
                    }),
                    Processing = processingJobs.Select(j => new
                    {
                        j.Key,
                        j.Value.Job.Method.Name,
                        j.Value.StartedAt
                    })
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).WithName("Recent Jobs");
    }
}