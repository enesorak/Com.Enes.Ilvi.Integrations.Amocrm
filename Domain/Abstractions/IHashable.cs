namespace Domain.Abstractions;

public interface IHashable
{
    public string? ComputedHash { get; set; }
}