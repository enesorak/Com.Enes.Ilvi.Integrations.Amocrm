using Domain.Abstractions;

namespace Domain.Pipelines;

public class Pipeline : Entity<PipelineId>
{
    public Pipeline(long id, string raw, string status) : base(new PipelineId(id))
    {
        Raw = raw;
        Status = status;
    }

    private Pipeline() : base()
    {
    }
    
    public string Raw { get; set; }
    public string Status { get; set; }
}