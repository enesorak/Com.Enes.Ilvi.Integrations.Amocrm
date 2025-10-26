using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Messages;

public class FetchMessagesBetweenDatesCommandHandler(
    IServiceHandler handler,
    IMessageRepository repository,
    ILogger<FetchMessagesBetweenDatesCommandHandler> logger) : ICommandHandler<FetchMessagesBetweenDatesCommand>
{
    private const int PageSize = 100;

    public async Task<Result> Handle(FetchMessagesBetweenDatesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var page = 1; 

            logger.LogInformation("Getting existing records.. ");
            var existingRecords = await repository.GetAllIdsAsync();
            var existingRecordsDictionary = existingRecords.ToDictionary
                (c => c);
            logger.LogInformation("Getting existing records. OK. ");


            var listToInsert = new List<Message>();

            var checkList = new List<MessageId>(); 
              
            var startDate = ((DateTimeOffset)request.StartDate).ToUnixTimeSeconds();
            var endDate = ((DateTimeOffset)request.EndDate).ToUnixTimeSeconds();
            while (true)
            {
                logger.LogInformation("Fetching contacts from service (Page {Page}, PageSize {PageSize})", page,
                    PageSize);
                var data = await handler.GetMessagesFromAsync(startDate,endDate, page, PageSize, cancellationToken);

                if (string.IsNullOrWhiteSpace(data))
                {
                    throw new Exception(
                        $"Error: Received empty or null data from serviceHandler.GetContactsAsync on page {page}");
                }

                logger.LogDebug("Raw data received: {Data}", data);

                EventResponse.Root? result;
                try
                {
                    result = JsonConvert.DeserializeObject<EventResponse.Root>(data);
                    if (result is null)
                    {
                        throw new Exception(
                            $"Error: Deserialization returned null for data on page {page}. Raw data: {data}");
                    }
                }
                catch (JsonSerializationException ex)
                {
                    throw new Exception($"Error: JSON deserialization failed for data on page {page}. Raw data: {data}",
                        ex);
                }


                foreach (var item in result.Embedded.Events)
                {
                    var id = item.Id;
                    var raw = JsonConvert.SerializeObject(item);
                    var eventType = item.Type;
                    var entityType = item.EntityType;
                    var entityId = item.EntityId;
                    var eventAtUtc = DateTimeOffset.FromUnixTimeSeconds(item.CreatedAt).DateTime;

                    if (existingRecordsDictionary.TryGetValue(item.Id, out _))
                    {
                        logger.LogInformation("Found existing event {id}, application continues", item.Id);
                        continue;
                    }

                    var create = new Message(id, eventType, entityId, entityType, raw, eventAtUtc);
                    create.ComputeHash(raw);
                    create.Inserted();
                    create.Checked();


                    if (listToInsert.Any(x => x.ComputedHash == create.ComputedHash))
                        continue;

                    if (checkList.Any(x => x.Value == create.Id.Value))
                        continue;


                    listToInsert.Add(create);


                    if (listToInsert.Count == 1000)
                    {
                        repository.BulkInsert(listToInsert.ToList());
                        // BackgroundJob.Enqueue(
                        //   () => repository.BulkInsert(listToInsert.ToList()));

                        checkList.AddRange(listToInsert.Select(x => x.Id)); 
                        listToInsert.Clear();
                    }
                }

                var nextPage = result.Links.Next;

                if (nextPage is null)
                    break;

                page++;
            }

            if (listToInsert.Count != 0) repository.BulkInsert(listToInsert.ToList());
            listToInsert.Clear();
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while fetching");

            return Result.Failure(new Error("Error while fetching", e.Message));
        }
    }
}