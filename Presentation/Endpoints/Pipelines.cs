using Hangfire;
using Presentation.BackgroundJobs;

namespace Presentation.Endpoints;

public static class Pipelines
{
    public static void MapPipelinesEndpoint(this IEndpointRouteBuilder endpoints)
    { 
        endpoints.MapPost("api/amocrm/pipelines",
            (IBackgroundJobClient backgroundJobs, bool deleteExisting = false) =>
            {
                backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                    executor.ExecuteFetchPipelines(deleteExisting));

                return Results.Ok("The job has been scheduled successfully.");
            }).WithName("Pipelines Background Job");
         
    }
}