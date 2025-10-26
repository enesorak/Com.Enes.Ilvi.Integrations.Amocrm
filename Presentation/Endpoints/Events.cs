using Application.Events;
using Application.Messages;
using Hangfire;
using MediatR;
using Presentation.BackgroundJobs;

namespace Presentation.Endpoints;

public static class Events
{
    public static void MapEventsEndpoint(this IEndpointRouteBuilder endpoints)
    {
     
        endpoints.MapPost($"api/amocrm/events", async (ISender sender,IConfiguration configuration) =>
        {
            
            var startDate = DateTime.UtcNow.AddDays(-50).ToString("yyyy-MM-dd HH:mm:ss");
            var myDateUtc = DateTime.SpecifyKind(DateTime.Parse(startDate!), DateTimeKind.Utc); 
            
            var request = new FetchEventsCommand(myDateUtc);
            var result = await sender.Send(request);

            return Results.Ok(result);
        }).WithName("Events"); 
        
       
        
        
          endpoints.MapPost("api/amocrm/messages", async (ISender sender, IConfiguration configuration) =>
        {
            var startDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss");
            var myDateUtc = DateTime.SpecifyKind(DateTime.Parse(startDate!), DateTimeKind.Utc);

            var request = new FetchMessagesCommand(myDateUtc);
            var result = await sender.Send(request);


            return Results.Ok(result);
        }).WithName("Messages");
          
           endpoints.MapPost("api/amocrm/events/time-interval", (IBackgroundJobClient backgroundJobs,string startDate, string endDate) =>
        {
              
            var sd = DateTime.SpecifyKind(DateTime.Parse(startDate), DateTimeKind.Utc);
            var ed = DateTime.SpecifyKind(DateTime.Parse(endDate), DateTimeKind.Utc);
            
            if (ed < sd)
                throw new ArgumentException("EndDate must be later than StartDate.");
            
            var differenceInDays = (ed - sd).Days;

            if (differenceInDays >= 15)
            {
                var jobStartDate = sd;
                var jobEndDate = sd.AddDays(15);
                string? parentJobId = null;

                while (jobStartDate < ed)
                {
                    // Enqueue the job, chaining it if a previous job exists
                    if (parentJobId == null)
                    {
                        // First job
                        var date = jobStartDate;
                        var date1 = jobEndDate;
                        parentJobId = backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => 
                            executor.ExecuteEventsBetweenDates(date, date1));
                    }
                    else
                    {
                        // Further jobs depend on their parent
                        var date = jobStartDate;
                        var date1 = jobEndDate;
                        parentJobId = backgroundJobs.ContinueJobWith<BackgroundJobExecutor>(parentJobId, executor => 
                            executor.ExecuteEventsBetweenDates(date, date1));
                    }

                    // Move to the next interval (15 days)
                    jobStartDate = jobEndDate;
                    jobEndDate = jobEndDate.AddDays(15) > ed ? ed : jobEndDate.AddDays(15); 
                }
            }
            else
            {
                // If the range is less than 15 days, schedule a single job
                backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => 
                    executor.ExecuteEventsBetweenDates(sd, ed));
            }
            
     
            

            return Results.Ok("The job has been scheduled successfully.");
        }).WithName("Events Background Job with Time Interval ");

        endpoints.MapPost("api/amocrm/messages/time-interval",
            (IBackgroundJobClient backgroundJobs, string startDate, string endDate) =>
            {
                var sd = DateTime.SpecifyKind(DateTime.Parse(startDate), DateTimeKind.Utc);
                var ed = DateTime.SpecifyKind(DateTime.Parse(endDate), DateTimeKind.Utc);

                if (ed < sd)
                    throw new ArgumentException("EndDate must be later than StartDate.");

                var differenceInDays = (ed - sd).Days;

                if (differenceInDays >= 15)
                {
                    var jobStartDate = sd;
                    var jobEndDate = sd.AddDays(15);
                    string? parentJobId = null;

                    while (jobStartDate < ed)
                    {
                        // Enqueue the job, chaining it if a previous job exists
                        if (parentJobId == null)
                        {
                            // First job
                            var date = jobStartDate;
                            var date1 = jobEndDate;
                            parentJobId = backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                                executor.ExecuteFetchMessages());
                        }
                        else
                        {
                            // Further jobs depend on their parent
                            var date = jobStartDate;
                            var date1 = jobEndDate;
                            parentJobId = backgroundJobs.ContinueJobWith<BackgroundJobExecutor>(parentJobId, executor =>
                                executor.ExecuteMessagesBetweenDates(date,date1));
                        }

                        // Move to the next interval (15 days)
                        jobStartDate = jobEndDate;
                        jobEndDate = jobEndDate.AddDays(15) > ed ? ed : jobEndDate.AddDays(15);
                    }
                }
                else
                {
                    // If the range is less than 15 days, schedule a single job
                    backgroundJobs.Enqueue<BackgroundJobExecutor>(executor =>
                        executor.ExecuteMessagesBetweenDates(sd, ed));
                }


                return Results.Ok("The job has been scheduled successfully.");
            }).WithName("Messages Background Job with Time Interval ");

    }
}