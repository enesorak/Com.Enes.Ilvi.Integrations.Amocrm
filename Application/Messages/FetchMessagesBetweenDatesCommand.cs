using Application.Abstractions.Messaging;

namespace Application.Messages;

public record FetchMessagesBetweenDatesCommand(DateTime StartDate, DateTime EndDate) : ICommand;
