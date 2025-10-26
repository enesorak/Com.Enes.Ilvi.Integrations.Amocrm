using Application.Abstractions.Messaging;

namespace Application.Pipelines;

public record FetchPipelinesCommand(bool DeleteExisting = false) : ICommand;
 