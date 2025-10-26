using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Tasks.TaskTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Tasks.TaskTypes;

public class FetchTaskTypesCommandHandler(
    IServiceHandler handler,
    ITaskTypeRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<FetchTaskTypesCommandHandler> logger) : ICommandHandler<FetchTaskTypesCommand>
{
    public async Task<Result> Handle(FetchTaskTypesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.DeleteExisting)
                await repository.TruncateAsync(cancellationToken);


            var existingContacts = await repository.GetAllAsync();
            var existingContactsDictionary = existingContacts.ToDictionary
                (c => c.Id.Value, c => c);

            var listToInsert = new List<TaskType>();
            var listToUpdate = new List<TaskType>();


            while (true)
            {
                var data = await handler.GetTaskTypes(cancellationToken);
                if (string.IsNullOrWhiteSpace(data))
                    throw new Exception($"Error: Received empty or null data: {data}");

                logger.LogDebug("Raw data received: {Data}", data);

                TaskTypeResponse.Root? result;
                try
                {
                    result = JsonConvert.DeserializeObject<TaskTypeResponse.Root>(data);
                    if (result is null)
                    {
                        throw new Exception($"Error: Deserialization returned null for data. Raw data: {data}");
                    }
                }
                catch (JsonSerializationException ex)
                {
                    throw new Exception($"Error: JSON deserialization failed for data. Raw data: {data}", ex);
                }

                foreach (var item in result.Embedded.TaskTypes)
                {
                    var id = item.Id;
                    var name = item.Name;
                    var color = item.Color;
                    var iconId = item.IconId;
                    var code = item.Code;

                    if (existingContactsDictionary.TryGetValue(item.Id, out var existingContact))
                    {
                        existingContact.Name = name;
                        existingContact.Color = color;
                        existingContact.IconId = iconId;
                        existingContact.Code = code;


                        existingContact.Checked();
                        existingContact.Updated();
                        listToUpdate.Add(existingContact);
                    }
                    else
                    {
                        var contact = new TaskType(id, name, color, iconId, code);

                        contact.Inserted();
                        contact.Checked();
                        listToInsert.Add(contact);
                    }
                }

                if (listToInsert.Count == 1000)
                {
                    repository.BulkInsert(listToInsert.ToList());
                    // BackgroundJob.Enqueue(
                    //   () => repository.BulkInsert(listToInsert.ToList()));

                    listToInsert.Clear();
                }

                if (listToUpdate.Count == 1000)
                {
                    repository.BulkUpdate(listToUpdate);
                    // BackgroundJob.Enqueue(
                    //   () => repository.BulkUpdate(listToUpdate.ToList()));
                    listToUpdate.Clear();
                }


                break;
            }

            if (listToInsert.Count != 0) repository.BulkInsert(listToInsert.ToList());
            if (listToUpdate.Count != 0) repository.BulkUpdate(listToUpdate.ToList());

            listToInsert.Clear();
            listToUpdate.Clear();

            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}