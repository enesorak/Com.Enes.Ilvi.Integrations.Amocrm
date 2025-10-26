using Application.Abstractions.Messaging;

namespace Application.Tasks;

public record FetchTasksCommand(bool DeleteExisting = false) : ICommand;