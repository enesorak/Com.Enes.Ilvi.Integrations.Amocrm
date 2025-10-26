using Application.Abstractions.Messaging;

namespace Application.Messages;

public record FetchMessagesCommand(DateTime StartDate) : ICommand;
