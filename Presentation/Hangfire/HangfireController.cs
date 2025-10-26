using Hangfire;
using Hangfire.Storage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.BackgroundJobs;

namespace Presentation.Hangfire;

[ApiController]
[Route("api/background-jobs")]
public class HangfireController(ISender sender, IBackgroundJobClient backgroundJobs, IConfiguration configuration)
    : ControllerBase
{
   
    [HttpGet("schedule-recurring-job")]
    public void SetRecurringJob(string cron = "0 0 * * *", bool deleteExisting = false)
    {
        RecurringJob.AddOrUpdate(
            "Job Recurring",
            () => EnqueueJobs(deleteExisting),
            cron);
    }
 
    [HttpGet("set-enqueue-job")]
    public void EnqueueJobs(bool deleteExisting = false)
    {
        backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => executor.ExecuteFetchTaskTypes(deleteExisting));
        backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => executor.ExecuteFetchUsers(deleteExisting));
        backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => executor.ExecuteFetchPipelines(deleteExisting));

        var contacts =
            backgroundJobs.Enqueue<BackgroundJobExecutor>(executor => executor.ExecuteFetchContacts(deleteExisting));

        var leads = backgroundJobs.ContinueJobWith<BackgroundJobExecutor>(contacts,
            executor => executor.ExecuteFetchLeads(deleteExisting));

        var tasks = backgroundJobs.ContinueJobWith<BackgroundJobExecutor>(leads,
            executor => executor.ExecuteFetchTasks(deleteExisting));


        var events = backgroundJobs.ContinueJobWith<BackgroundJobExecutor>(tasks, executor => executor.ExecuteEvents());
        var messages =
            backgroundJobs.ContinueJobWith<BackgroundJobExecutor>(tasks, executor => executor.ExecuteFetchMessages());
    }

    [HttpGet("remove-recurring-jobs")]
    public void RemoveRecurringJobs()
    {
        using var connection = JobStorage.Current.GetConnection();
        foreach (var recurringJob in connection.GetRecurringJobs())
        {
            RecurringJob.RemoveIfExists(recurringJob.Id);
        }
    }
}