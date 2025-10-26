using Hangfire;
using Presentation.BackgroundJobs;

namespace Presentation.Endpoints;

public static class Leads
{
    public static void MapLeadsEndpoint(this IEndpointRouteBuilder endpoints)
    { 
        endpoints.MapPost("api/amocrm/leads", (IBackgroundJobClient backgroundJobs, bool deleteExisting = false) =>
        {
            
            backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => 
                executor.ExecuteFetchLeads(deleteExisting));

            return Results.Ok("The job has been scheduled successfully.");
        }).WithName("Leads Background Job");
        
    }
}