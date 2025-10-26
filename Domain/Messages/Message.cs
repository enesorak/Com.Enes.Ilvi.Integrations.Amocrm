using Domain.Abstractions;

namespace Domain.Messages;

public class Message :Entity <MessageId>
{
    public Message(string id, string eventType, long entityId, string entityType, string raw, DateTime eventAtUtc) : base(new MessageId(id))
    {
        EventType = eventType;
        EntityId = entityId;
        EntityType = entityType;
        Raw = raw;
        EventAtUtc = eventAtUtc;
    }


    private Message()
    {
        
    }
    
    public string EventType { get; set; }
    public long EntityId { get; set; }
    public string EntityType { get; set; }

    public string Raw { get; set; }

    public DateTime EventAtUtc { get; set; }
}