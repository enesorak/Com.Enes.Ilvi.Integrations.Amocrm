using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Pipelines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Pipelines;

public class FetchPipelinesCommandHandler(IServiceHandler handler, IPipelineRepository repository, ILogger<FetchPipelinesCommandHandler> logger) : ICommandHandler<FetchPipelinesCommand>
{
    public async Task<Result> Handle(FetchPipelinesCommand request, CancellationToken cancellationToken)
    {
       try
        {
            if (request.DeleteExisting)
                await repository.TruncateAsync(cancellationToken); 
            
            var existingRecords = await repository.GetAllAsync();
            var existingRecordsDictionary = existingRecords.ToDictionary
                (c => c.Id.Value, c => c);

            var listToInsert = new List<Pipeline>();
            var listToUpdate = new List<Pipeline>();


            while (true)
            {
                var data = await handler.GetPipelinesAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(data))
                    throw new Exception($"Error: Received empty or null data: {data}");

                logger.LogDebug("Raw data received: {Data}", data);

                PipelineResponse.Root? result;
                try
                {
                    result = JsonConvert.DeserializeObject<PipelineResponse.Root>(data);
                    if (result is null)
                    {
                        throw new Exception($"Error: Deserialization returned null for data. Raw data: {data}");
                    }
                }
                catch (JsonSerializationException ex)
                {
                    throw new Exception($"Error: JSON deserialization failed for data. Raw data: {data}", ex);
                }

                foreach (var item in result.Embedded.Pipelines)
                {
                    var id = item.Id;
                    var raw = JsonConvert.SerializeObject(item);
                    var status = JsonConvert.SerializeObject(item.Embedded.Statuses);

                    if (existingRecordsDictionary.TryGetValue(item.Id, out var existing))
                    {
                        if (existing.IsHashEqual(raw))
                        {
                            existing.Checked();
                            listToUpdate.Add(existing);
                            continue;
                        } 
                        
                        existing.Raw = raw;
                        existing.Status = status;
                        existing.ComputeHash(raw);
                        existing.Checked();
                        existing.Updated();
                        listToUpdate.Add(existing);
                    }
                    else
                    {
                        var create = new Pipeline(id, raw, status);

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