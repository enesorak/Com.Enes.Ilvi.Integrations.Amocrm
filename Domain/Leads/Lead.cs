using Domain.Abstractions;

namespace Domain.Leads;

public class Lead : Entity<LeadId>
{
    public Lead(long id, string raw, string company, string tag, DateTime? sourceUpdatedAtUtc) : base(new LeadId(id))
    {
        Raw = raw;
        Company = company;
        Tag = tag;
        SourceUpdatedAtUtc = sourceUpdatedAtUtc;
    }

    private Lead() : base()
    {
    }
    
    public string Raw { get; set; }

    public string Company { get; set; }
    public string Tag { get; set; }
}