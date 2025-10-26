using Domain.Abstractions;

namespace Domain.Tasks.TaskTypes;

public class TaskType : Entity<TaskTypeId>
{
    private TaskType() : base()
    {
    }


    public string Name { get; set; }
    public string? Color { get; set; }
    public long? IconId { get; set; }
    public string Code { get; set; }
    
    
    
    public TaskType(long id, string name, string color, long? iconId, string code)
        : base(new TaskTypeId(id)) 
    {
        Name = name;
        Color = color;
        IconId = iconId;
        Code = code;
    } 
    
}