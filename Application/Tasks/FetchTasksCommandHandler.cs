using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Tasks;

public class FetchTasksCommandHandler(
    IServiceHandler handler,
    ITaskRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<FetchTasksCommandHandler> logger) : ICommandHandler<FetchTasksCommand>
{
    
    private const int PageSize = 100;
    public async Task<Result> Handle(FetchTasksCommand request, CancellationToken cancellationToken)
    {
          try
          {
              var page = 1;

              if (request.DeleteExisting)
              {
                  await repository.TruncateAsync(cancellationToken);
              }

              logger.LogInformation("Getting existing tasks.. ");
              var existingRecords = await repository.GetAllAsync();
              var existingRecordsDictionary = existingRecords.ToDictionary
                  (c => c.Id.Value, c => c);
              logger.LogInformation("Getting existing tasks. OK. ");


              var listToInsert = new List<CTask>();
              var listToUpdate = new List<CTask>();

              while (true)
              {
                  logger.LogInformation("Fetching contacts from service (Page {Page}, PageSize {PageSize})", page, PageSize);
                  var data = await handler.GetTasksAsync(page, PageSize, cancellationToken);
                
                  if (string.IsNullOrWhiteSpace(data))
                  {
                      throw new Exception($"Error: Received empty or null data from serviceHandler.GetContactsAsync on page {page}");
                  }
                  logger.LogDebug("Raw data received: {Data}", data);
                 
                
                  TasksResponse.Root? result;
                  try
                  {
                      result = JsonConvert.DeserializeObject<TasksResponse.Root>(data);
                      if (result is null)
                      {
                          throw new Exception($"Error: Deserialization returned null for data on page {page}. Raw data: {data}");
                      }
                  }
                  catch (JsonSerializationException ex)
                  {
                      throw new Exception($"Error: JSON deserialization failed for data on page {page}. Raw data: {data}", ex);
                  }
              

               

                  foreach (var item in result.Embedded.Tasks)
                  {
                      var id = item.Id; 
                      
                      var raw = JsonConvert.SerializeObject(item);

                      
                      var sourceUpdatedAtUtc = item.UpdatedAt is null ? (DateTime?)null :  DateTimeOffset.FromUnixTimeSeconds((long)item.UpdatedAt).UtcDateTime;

                      if (existingRecordsDictionary.TryGetValue(item.Id, out var existing))
                      {
                          if (existing.IsHashEqual(raw))
                          {
                              existing.Checked();
                              listToUpdate.Add(existing);
                              continue;
                          } 
                          existing.Raw = raw;
                        
                          existing.SourceUpdatedAtUtc = sourceUpdatedAtUtc;

                          existing.ComputeHash(raw);
                          existing.Checked();
                          existing.Updated();
                          listToUpdate.Add(existing);
                      }
                      else
                      {
                          var create = new CTask(id, raw, sourceUpdatedAtUtc); 

                          create.ComputeHash(raw);
                          create.Inserted();
                          create.Checked();
                          listToInsert.Add(create);
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


                  var nextPage = result.Links.Next;

                  if (nextPage is null)
                      break;

                  page++;
                  
              }

              if (listToInsert.Count != 0) repository.BulkInsert(listToInsert.ToList());
              // await repository.AddRangeAsync(listToInsert.ToList(),CancellationToken.None);
              if (listToUpdate.Count != 0) repository.BulkUpdate(listToUpdate.ToList());
              //repository.UpdateRange(listToUpdate.ToList());
              // await unitOfWork.SaveChangesAsync(cancellationToken);

              listToInsert.Clear();
              listToUpdate.Clear();

              return Result.Success();
          }
          catch (Exception e)
          {
              logger.LogError(e, "An error occurred while fetching");

              return Result.Failure(new Error("Error while fetching", e.Message)); 
          }
    }
}