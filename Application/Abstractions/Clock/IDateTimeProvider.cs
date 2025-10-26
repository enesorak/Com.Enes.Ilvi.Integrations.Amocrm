namespace Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}