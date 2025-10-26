using Application.Abstractions.Messaging;

namespace Application.Contacts.Fetch;

public record FetchContactCommand(long Id) : ICommand;