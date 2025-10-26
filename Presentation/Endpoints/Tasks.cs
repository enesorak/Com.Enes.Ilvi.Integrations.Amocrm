using Hangfire;
using Presentation.BackgroundJobs;

namespace Presentation.Endpoints;

public static class Tasks
{
    public static void MapTasksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/amocrm/tasks",
            (IBackgroundJobClient backgroundJobs, bool deleteExisting = false) =>
            {
                backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                    executor.ExecuteFetchTasks(deleteExisting));

                return Results.Ok("The job has been scheduled successfully.");
            }).WithName("Tasks Background Job");

        endpoints.MapPost("api/amocrm/task-types",
            (IBackgroundJobClient backgroundJobs, bool deleteExisting = false) =>
            {
                backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                    executor.ExecuteFetchTaskTypes(deleteExisting));

                return Results.Ok("The job has been scheduled successfully.");
            }).WithName("Task Types Background Job");
    }
}