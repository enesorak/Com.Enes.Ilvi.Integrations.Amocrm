using Application.Abstractions.Messaging;

namespace Application.Tasks.TaskTypes;

public record FetchTaskTypesCommand(bool DeleteExisting = false) : ICommand;