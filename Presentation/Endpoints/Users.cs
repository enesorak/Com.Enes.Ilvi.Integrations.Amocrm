using Hangfire;
using Presentation.BackgroundJobs;

namespace Presentation.Endpoints;

public static class Users
{
    public static void MapUsersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/amocrm/users",
            (IBackgroundJobClient backgroundJobs, bool deleteExisting = false) =>
            {
                backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                    executor.ExecuteFetchUsers(deleteExisting));

                return Results.Ok("The job has been scheduled successfully.");
            }).WithName("Users Background Job");
    }
}