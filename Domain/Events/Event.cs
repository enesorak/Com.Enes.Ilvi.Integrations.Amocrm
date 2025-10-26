using Domain.Abstractions;

namespace Domain.Events;

public class Event : Entity<EventId>
{
    public Event(string id, string eventType, long entityId, string entityType, string raw, DateTime eventAtUtc) : base(
        new EventId(id))
    {
        EventType = eventType;
        EntityId = entityId;
        EntityType = entityType;
        Raw = raw;
        EventAtUtc = eventAtUtc;
    }

    private Event()
    {
    }

    public string EventType { get; set; }
    public long EntityId { get; set; }
    public string EntityType { get; set; }

    public string Raw { get; set; }

    public DateTime EventAtUtc { get; set; }
}