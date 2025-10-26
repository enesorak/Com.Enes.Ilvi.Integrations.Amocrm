using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Contacts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Contacts.Fetch;

public class FetchContactsCommandHandler(
    IServiceHandler serviceHandler,
    IContactRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<FetchContactsCommandHandler> logger)
    : ICommandHandler<FetchContactsCommand>
{
    private const int PageSize = 100;

    public async Task<Result> Handle(FetchContactsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var page = 1;

            if (request.DeleteExisting)
            {
                await repository.TruncateAsync(cancellationToken);
            }

            logger.LogInformation("Getting existing contacts.. ");
            var existingContacts = await repository.GetAllAsync();
            var existingContactsDictionary = existingContacts.ToDictionary
                (c => c.Id.Value, c => c);
            logger.LogInformation("Getting existing contacts. OK. ");


            var listToInsert = new List<Contact>();
            var listToUpdate = new List<Contact>();

            while (true)
            {
                logger.LogInformation("Fetching contacts from service (Page {Page}, PageSize {PageSize})", page, PageSize);
                var data = await serviceHandler.GetContactsAsync(page, PageSize, cancellationToken);
                
                if (string.IsNullOrWhiteSpace(data))
                {
                    throw new Exception($"Error: Received empty or null data from serviceHandler.GetContactsAsync on page {page}");
                }
                logger.LogDebug("Raw data received: {Data}", data);
                ContactsResponse.Root? result;
                try
                {
                    result = JsonConvert.DeserializeObject<ContactsResponse.Root>(data);
                    if (result is null)
                    {
                        throw new Exception($"Error: Deserialization returned null for data on page {page}. Raw data: {data}");
                    }
                }
                catch (JsonSerializationException ex)
                {
                    throw new Exception($"Error: JSON deserialization failed for data on page {page}. Raw data: {data}", ex);
                }
              

               

                foreach (var item in result.Embedded.Contacts)
                {
                    var id = item.Id;
                    var lead = JsonConvert.SerializeObject(item.Embedded.Leads);
                    var company = JsonConvert.SerializeObject(item.Embedded.Companies);
                    var tag = JsonConvert.SerializeObject(item.Embedded.Tags);
                    var raw = JsonConvert.SerializeObject(item);

                    var sourceUpdatedAtUtc = DateTimeOffset.FromUnixTimeSeconds(item.UpdatedAt).UtcDateTime;

                    if (existingContactsDictionary.TryGetValue(item.Id, out var existingContact))
                    {
                        if (existingContact.IsHashEqual(raw))
                        {
                            existingContact.Checked();
                            listToUpdate.Add(existingContact);
                            continue;
                        } 
                        existingContact.Raw = raw;
                        existingContact.Lead = lead;
                        existingContact.Company = company;
                        existingContact.Tag = tag;
                        existingContact.SourceUpdatedAtUtc = sourceUpdatedAtUtc;

                        existingContact.ComputeHash(raw);
                        existingContact.Checked();
                        existingContact.Updated();
                        listToUpdate.Add(existingContact);
                    }
                    else
                    {
                        var contact = new Contact(id, raw, lead, company, tag)
                        {
                            SourceUpdatedAtUtc = sourceUpdatedAtUtc
                        };

                        contact.ComputeHash(raw);
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
            logger.LogError(e, "An error occurred while fetching contacts");

            return Result.Failure(new Error("Error while fetching contacts", e.Message)); 
        }
    }
    
    
}