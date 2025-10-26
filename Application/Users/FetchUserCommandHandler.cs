using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Users;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Users;

public class FetchUserCommandHandler(
    IServiceHandler handler,
    IUserRepository repository,
    ILogger<FetchUserCommandHandler> logger) : ICommandHandler<FetchUserCommand>
{
    public async Task<Result> Handle(FetchUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.DeleteExisting)
                await repository.TruncateAsync(cancellationToken); 
            
            var existingRecords = await repository.GetAllAsync();
            var existingRecordsDictionary = existingRecords.ToDictionary
                (c => c.Id.Value, c => c);

            var listToInsert = new List<User>();
            var listToUpdate = new List<User>();


            while (true)
            {
                var data = await handler.GetUsersAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(data))
                    throw new Exception($"Error: Received empty or null data: {data}");

                logger.LogDebug("Raw data received: {Data}", data);

                UsersResponse.Root? result;
                try
                {
                    result = JsonConvert.DeserializeObject<UsersResponse.Root>(data);
                    if (result is null)
                    {
                        throw new Exception($"Error: Deserialization returned null for data. Raw data: {data}");
                    }
                }
                catch (JsonSerializationException ex)
                {
                    throw new Exception($"Error: JSON deserialization failed for data. Raw data: {data}", ex);
                }

                foreach (var item in result.Embedded.Users)
                {
                    var id = item.Id;
                    var raw = JsonConvert.SerializeObject(item);

                    if (existingRecordsDictionary.TryGetValue(item.Id, out var existing))
                    {
                        if (existing.IsHashEqual(raw))
                        {
                            existing.Checked();
                            listToUpdate.Add(existing);
                            continue;
                        } 
                        
                        existing.Raw = raw;
                        existing.ComputeHash(raw);
                        existing.Checked();
                        existing.Updated();
                        listToUpdate.Add(existing);
                    }
                    else
                    {
                        var create = new User(id, raw);

                        create.ComputeHash(raw);
                        create.Inserted();
                        create.Checked();
                        listToInsert.Add(create);
                    }
                }

                if (listToInsert.Count == 1000)
                {
                    repository.BulkInsert(listToInsert.ToList());
                    listToInsert.Clear();
                }

                if (listToUpdate.Count == 1000)
                {
                    repository.BulkUpdate(listToUpdate);
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