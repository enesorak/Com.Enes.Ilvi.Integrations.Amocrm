using Application.Abstractions.Messaging;

namespace Application.Events;

public record FetchEventsBetweenStartDateAndEndDateCommand(DateTime StartDate, DateTime EndDate) : ICommand;
