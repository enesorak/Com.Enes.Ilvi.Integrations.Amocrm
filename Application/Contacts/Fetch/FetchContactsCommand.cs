using Application.Abstractions.Messaging;

namespace Application.Contacts.Fetch;

public record FetchContactsCommand(bool DeleteExisting = false) : ICommand;