namespace Application.Amocrm;

public interface IServiceHandler
{
    Task<string>
        GetContactsAsync(int page, int limit, CancellationToken cancellationToken);
    
    Task<string>
        GetContactByIdAsync(long id, CancellationToken cancellationToken);

    Task<string>
        GetTasksAsync(int page, int limit, CancellationToken cancellationToken);

    Task<string>
        UrlTesting(string url,CancellationToken cancellationToken);
    Task<string>
        GetLeadsAsync(int page, int limit, CancellationToken cancellationToken);

    Task<string>
        GetEventsAsync(int page, int limit, CancellationToken cancellationToken);

    Task<string>
        GetEventsFromAsync(long timestamp, int page, int limit, CancellationToken cancellationToken);
    
    Task<string>
        GetEventsFromAsync(long startDate, long endDate, int page, int limit, CancellationToken cancellationToken);
    
    Task<string>
        GetMessagesFromAsync(long startDate, long endDate, int page, int limit, CancellationToken cancellationToken);

    Task<string>
        GetMessagesFromAsync(long timestamp, int page, int limit, CancellationToken cancellationToken);

    Task<string>
        GetPipelinesAsync(CancellationToken cancellationToken);

    Task<string>
        GetUsersAsync(CancellationToken cancellationToken);

    Task<string>
        GetTaskTypes(CancellationToken cancellationToken);
}