using Domain.Abstractions;

namespace Domain.Tasks;



public class CTask :Entity<TaskId>
{
    public CTask(long id, string raw, DateTime? sourceUpdatedAtUtc) : base(new TaskId(id))
    {
        Raw = raw;
        SourceUpdatedAtUtc =sourceUpdatedAtUtc;
    }
    
    private CTask() : base()
    {
    }
        
    public string Raw { get; set; }  
}