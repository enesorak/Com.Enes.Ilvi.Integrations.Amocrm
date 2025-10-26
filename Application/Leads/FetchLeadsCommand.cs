using Application.Abstractions.Messaging;

namespace Application.Leads;

public record FetchLeadsCommand(bool DeleteExisting = false) : ICommand;