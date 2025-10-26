using Application.Contacts.Fetch;
using Application.Events;
using Application.Leads;
using Application.Messages;
using Application.Pipelines;
using Application.Tasks;
using Application.Tasks.TaskTypes;
using Application.Users;
using Hangfire;
using MediatR;

namespace Presentation.BackgroundJobs;

public class BackgroundJobExecutor(ISender sender, ILogger<BackgroundJobExecutor> logger, IConfiguration configuration)
{
    [JobDisplayName("Fetch Users")]
    public async Task ExecuteFetchUsers(bool deleteExisting)
    {
        logger.LogInformation("Fetch Users");
        var request = new FetchUserCommand(deleteExisting);
        await sender.Send(request);
    }


    [JobDisplayName("Fetch Contacts")]
    public async Task ExecuteFetchContacts(bool deleteExisting)
    {
        logger.LogInformation("Fetch Contacts");
        var request = new FetchContactsCommand(deleteExisting);
        await sender.Send(request);
    }

    [JobDisplayName("Fetch Pipelines")]
    public async Task ExecuteFetchPipelines(bool deleteExisting)
    {
        logger.LogInformation("Fetch Pipelines");
        var request = new FetchPipelinesCommand(deleteExisting);
        await sender.Send(request);
    }

    [JobDisplayName("Fetch Tasks")]
    public async Task ExecuteFetchTasks(bool deleteExisting)
    {
        logger.LogInformation("Fetch Tasks");
        var request = new FetchTasksCommand(deleteExisting);
        await sender.Send(request);
    }

    [JobDisplayName("Fetch TaskTypes")]
    public async Task ExecuteFetchTaskTypes(bool deleteExisting)
    {
        logger.LogInformation("Fetch TaskTypes");
        var request = new FetchTaskTypesCommand(deleteExisting);
        await sender.Send(request);
    }

    [JobDisplayName("Fetch Messages")]
    public async Task ExecuteFetchMessages()
    {
        logger.LogInformation("Fetch Messages");
        var request = new FetchMessagesCommand(GetStartDate());
        await sender.Send(request);
    }

    [JobDisplayName("Fetch Events")]
    public async Task ExecuteEvents()
    {
        logger.LogInformation("Fetch Events");
        var request = new FetchEventsCommand(GetStartDate());
        await sender.Send(request);
    }

    [JobDisplayName("Fetch Events between dates {0}-{1} ")]
    public async Task ExecuteEventsBetweenDates(DateTime startDate, DateTime endDate)
    {
        logger.LogInformation("Fetch Events");
        var request = new FetchEventsBetweenStartDateAndEndDateCommand(startDate, endDate);
        await sender.Send(request);
    }

    [JobDisplayName("Fetch Messages between dates {0}-{1} ")]
    public async Task ExecuteMessagesBetweenDates(DateTime startDate, DateTime endDate)
    {
        logger.LogInformation("Fetch Messages");
        var request = new FetchMessagesBetweenDatesCommand(startDate, endDate);
        await sender.Send(request);
    }


    [JobDisplayName("Fetch Leads")]
    public async Task ExecuteFetchLeads(bool deleteExisting)
    {
        logger.LogInformation("Fetch Leads");
        var request = new FetchLeadsCommand(deleteExisting);
        await sender.Send(request);
    }


    private static DateTime GetStartDate()
    {
        var startDate = DateTime.UtcNow.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss");
        return DateTime.SpecifyKind(DateTime.Parse(startDate), DateTimeKind.Utc);
    }
}