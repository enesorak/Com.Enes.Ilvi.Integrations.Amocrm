using Application.Amocrm;

namespace Infrastructure.Service;

public class ServiceHandler(AmoCrmService amoCrmService) : IServiceHandler
{
    
    public async Task<string> UrlTesting(string url, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(url, cancellationToken);
    }
    
    
    
    public async Task<string>
        GetContactsAsync(int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.ContactsUri(page, limit), cancellationToken);
    }

    public async Task<string> GetContactByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.ContactByIdUri(id), cancellationToken);
    }


    public async Task<string> GetTasksAsync(int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.TasksUri(page, limit), cancellationToken);
    }

 

    public async Task<string>
        GetLeadsAsync(int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.LeadsUri(page, limit), cancellationToken);
    }

    public async Task<string>
        GetEventsAsync(int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.EventsUri(page, limit), cancellationToken);
    }

    public async Task<string>
        GetEventsFromAsync(long timestamp, int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.EventsFromUriFiltered(timestamp, page, limit),
            cancellationToken);
    }

    public async Task<string> GetEventsFromAsync(long startDate, long endDate, int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.EventsFromUriBetweenDates(startDate,endDate, page, limit),
            cancellationToken);
    }

    public async Task<string> GetMessagesFromAsync(long startDate, long endDate, int page, int limit, CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.MessagesFromUriUriBetweenDates(startDate,endDate, page, limit),
            cancellationToken);
    }

    public async Task<string> GetMessagesFromAsync(long timestamp, int page, int limit,
        CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.MessagesFromUriFiltered(timestamp, page, limit),
            cancellationToken);
    }

    public async Task<string>
        GetPipelinesAsync(CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.Pipeline, cancellationToken);
    }

    public async Task<string> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.UsersUri, cancellationToken);
    }

    public async Task<string> GetTaskTypes(CancellationToken cancellationToken)
    {
        return await amoCrmService.GetAsync(Endpoints.TaskTypesUri, cancellationToken);
    }
}