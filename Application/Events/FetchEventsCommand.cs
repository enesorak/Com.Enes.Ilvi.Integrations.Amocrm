using Application.Abstractions.Messaging;

namespace Application.Events;

public record FetchEventsCommand(DateTime StartDate) : ICommand;
