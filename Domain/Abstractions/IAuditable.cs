namespace Domain.Abstractions;

public interface IAuditable
{
    public DateTime  CreatedAtUtc { get; set; }
    public DateTime? CheckedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}