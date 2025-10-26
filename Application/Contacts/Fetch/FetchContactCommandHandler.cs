using Application.Abstractions.Messaging;
using Application.Amocrm;
using Domain.Abstractions;
using Domain.Contacts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Contacts.Fetch;

public class FetchContactCommandHandler(IServiceHandler serviceHandler,IContactRepository contactRepository, IUnitOfWork unitOfWork,ILogger<FetchContactCommand> logger)
    : ICommandHandler<FetchContactCommand>
{
    public async Task<Result> Handle(FetchContactCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Getting existing contact {id}.. ", request.Id);

            var data = await serviceHandler.GetContactByIdAsync(request.Id, cancellationToken);

            if (string.IsNullOrWhiteSpace(data))
                throw new Exception(
                    $"Error: Received empty or null data from serviceHandler.GetContactByIdAsync {request.Id}");
            ContactResponse.Root? result;
            try
            { 
                result = JsonConvert.DeserializeObject<ContactResponse.Root>(data);
                if (result is null)
                {
                    throw new Exception($"Error: Deserialization returned null for id {request.Id}. Raw data: {data}");
                }
            }
            catch (JsonSerializationException ex)
            {
                throw new Exception($"Error: JSON deserialization failed for  for id {request.Id}. Raw data: {data}",
                    ex);
            }
           
            var id = result.Id;
            var lead = JsonConvert.SerializeObject(result.Embedded.Leads);
            var company = JsonConvert.SerializeObject(result.Embedded.Companies);
            var tag = JsonConvert.SerializeObject(result.Embedded.Tags);
            var raw = JsonConvert.SerializeObject(result);

            var sourceUpdatedAtUtc = DateTimeOffset.FromUnixTimeSeconds(result.UpdatedAt).UtcDateTime;

            var existingContact = await contactRepository.GetByIdAsync(new ContactId(id));

            if (existingContact is null)
            {
                var contact = new Contact(id, raw, lead, company, tag)
                {
                    SourceUpdatedAtUtc = sourceUpdatedAtUtc
                };
                
                contact.ComputeHash(raw);
                contact.Inserted();
                contact.Checked();
               
                await contactRepository.AddAsync(contact, cancellationToken);
            }
            else
            {
                if (existingContact.IsHashEqual(raw))
                {
                    existingContact.Checked(); 
                }
                else
                {
                    existingContact.Raw = raw;
                    existingContact.Lead = lead;
                    existingContact.Company = company;
                    existingContact.Tag = tag;
                    existingContact.SourceUpdatedAtUtc = sourceUpdatedAtUtc; 
                    existingContact.ComputeHash(raw);
                    existingContact.Checked();
                    existingContact.Updated();
                }
                
                contactRepository.Update(existingContact); 
            }
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            

            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}