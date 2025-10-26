using Application.Contacts.Fetch;
using Hangfire;
using MediatR;
using Presentation.BackgroundJobs;

namespace Presentation.Endpoints;

public static class Contacts
{
    public static void MapContactsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/amocrm/contacts",
            (IBackgroundJobClient backgroundJobs, bool deleteExisting = false) =>
            {
                backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                    executor.ExecuteFetchContacts(deleteExisting));

                return Results.Ok("The job has been scheduled successfully.");
            }).WithName("Contacts Background Job");


        endpoints.MapPost("api/amocrm/contacts/{id:long}",
            async (long id, ISender sender, bool deleteExisting = false) =>
            {
                var request = new FetchContactCommand(id);
                var result = await sender.Send(request);

                return Results.Ok(result);
            }).WithName("ContactById");
    }
}