using System.Security.Cryptography;
using System.Text;

namespace Domain.Abstractions;

public class Entity<TEntityId> : IEntity, IHashable, IAuditable
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(TEntityId id)
    {
        Id = id;
        CreatedAtUtc = DateTime.UtcNow;
    }

    protected Entity()
    {
    }

    public Entity<TEntityId> Inserted()
    {
        CreatedAtUtc = DateTime.UtcNow;
        return this;
    }

    public Entity<TEntityId> Checked()
    {
        CheckedAtUtc = DateTime.UtcNow;
        // update contact custom field.. 
        return this;
    }

    public Entity<TEntityId> Updated()
    {
        UpdatedAtUtc = DateTime.UtcNow;
        return this;
    }


    public Entity<TEntityId> ComputeHash(string subject)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(subject));
        ComputedHash = Convert.ToHexStringLower(hashBytes);
        return this;
    }

    public bool IsHashEqual(string subject)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(subject));
        return ComputedHash == Convert.ToHexStringLower(hashBytes);
    }


    public TEntityId Id { get; set; }


    public string? ComputedHash { get; set; }

    public DateTime? SourceUpdatedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? CheckedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();
}