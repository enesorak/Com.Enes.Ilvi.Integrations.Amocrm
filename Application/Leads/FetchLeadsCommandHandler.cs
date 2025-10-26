using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Leads;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Leads;

public class FetchLeadsCommandHandler(
    IServiceHandler handler,
    ILeadRepository repository,
    ILogger<FetchLeadsCommandHandler> logger) : ICommandHandler<FetchLeadsCommand>
{
    const int PageSize = 100;

    public async Task<Result> Handle(FetchLeadsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching leads.. ");
            var page = 1;

            if (request.DeleteExisting)
            {
                await repository.TruncateAsync(cancellationToken);
            }

            logger.LogInformation("Getting existing records.. ");
            var existingRecords = await repository.GetAllAsync();
            var existingRecordsDictionary = existingRecords.ToDictionary
                (c => c.Id.Value, c => c);
            logger.LogInformation("Getting existing records. OK. ");


            var listToInsert = new List<Lead>();
            var listToUpdate = new List<Lead>();

            while (true)
            {
                logger.LogInformation("Fetching contacts from service (Page {Page}, PageSize {PageSize})", page,
                    PageSize);
                var data = await handler.GetLeadsAsync(page, PageSize, cancellationToken);

                if (string.IsNullOrWhiteSpace(data))
                {
                    throw new Exception(
                        $"Error: Received empty or null data from serviceHandler.GetContactsAsync on page {page}");
                }

                logger.LogDebug("Raw data received: {Data}", data);

                LeadsResponse.Root? result;
                try
                {
                    result = JsonConvert.DeserializeObject<LeadsResponse.Root>(data);
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


                foreach (var item in result.Embedded.Leads)
                {
                    logger.LogDebug("Processing lead: {Lead}", item.Id);
                    var id = item.Id;
                    var raw = JsonConvert.SerializeObject(item);
                    var company = JsonConvert.SerializeObject(item.Embedded.Companies);
                    var tag = JsonConvert.SerializeObject(item.Embedded.Tags);
                    var sourceUpdatedAtUtc = item.UpdatedAt is null
                        ? (DateTime?)null
                        : DateTimeOffset.FromUnixTimeSeconds((long)item.UpdatedAt).UtcDateTime;

                    if (existingRecordsDictionary.TryGetValue(item.Id, out var existing))
                    {
                        logger.LogDebug("Lead {Lead} already exists. Checking for updates..", item.Id);
                        if (existing.IsHashEqual(raw))
                        {
                            logger.LogDebug("Lead {Lead} already exists. No updates found.", item.Id);
                            existing.Checked();
                            listToUpdate.Add(existing);
                            continue;
                        }

                        logger.LogDebug("Lead {Lead} already exists. Updates found.", item.Id);
                        existing.Raw = raw;
                        existing.SourceUpdatedAtUtc = sourceUpdatedAtUtc;

                        existing.ComputeHash(raw);
                        existing.Checked();
                        existing.Updated();
                        listToUpdate.Add(existing);
                    }
                    else
                    {
                        logger.LogDebug("Lead {Lead} does not exist. Inserting..", item.Id);
                        var create = new Lead(id, raw, company, tag, sourceUpdatedAtUtc);

                        create.ComputeHash(raw);
                        create.Inserted();
                        create.Checked();
                        listToInsert.Add(create);
                    }
                }

                if (listToInsert.Count == 1000)
                {
                    logger.LogDebug("Inserting {Count} leads", listToInsert.Count);
                    repository.BulkInsert(listToInsert.ToList());
                    // BackgroundJob.Enqueue(
                    //   () => repository.BulkInsert(listToInsert.ToList()));

                    listToInsert.Clear();

                    logger.LogDebug("Inserting {Count} leads. OK.", listToInsert.Count);
                }

                if (listToUpdate.Count == 1000)
                {
                    logger.LogDebug("Updating {Count} leads", listToUpdate.Count);
                    repository.BulkUpdate(listToUpdate);
                    // BackgroundJob.Enqueue(
                    //   () => repository.BulkUpdate(listToUpdate.ToList()));
                    listToUpdate.Clear();
                    logger.LogDebug("Updating {Count} leads. OK.", listToUpdate.Count);
                }

                var nextPage = result.Links.Next;
                logger.LogDebug("Next page: {NextPage}", nextPage);
                if (nextPage is null)
                    break;
               
                page++;
                logger.LogDebug("Page {Page} processed", page);
            }
            

            
            if (listToInsert.Count != 0) repository.BulkInsert(listToInsert.ToList());
            // await repository.AddRangeAsync(listToInsert.ToList(),CancellationToken.None);
            if (listToUpdate.Count != 0) repository.BulkUpdate(listToUpdate.ToList());
            //repository.UpdateRange(listToUpdate.ToList());
            // await unitOfWork.SaveChangesAsync(cancellationToken);

            listToInsert.Clear();
            listToUpdate.Clear();
            logger.LogInformation("Fetching leads. OK. ");
            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while fetching"); 
            return Result.Failure(new Error("Error while fetching", e.Message));
        }
    }
}