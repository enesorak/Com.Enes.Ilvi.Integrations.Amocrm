using Domain.Abstractions;

namespace Domain.Contacts;

public class Contact 
    : Entity<ContactId> 
{ 
    
    public Contact(long id, string raw, string lead, string company, string tag)
        : base(new ContactId(id)) 
    {
        Raw = raw ?? throw new ArgumentNullException(nameof(raw));
        Lead = lead ?? throw new ArgumentNullException(nameof(lead));
        Company = company ?? throw new ArgumentNullException(nameof(company));
        Tag = tag ?? throw new ArgumentNullException(nameof(tag));
    
        CreatedAtUtc = DateTime.UtcNow;
    }
    
    private Contact() : base()
    {
    }
   
    public string Raw { get; set; }  
    public string Lead { get; set; } 
    public string Company { get; set; }  
    public string Tag { get; set; }   
  
}