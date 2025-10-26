using Application.Abstractions.Messaging;

namespace Application.Users;

public record FetchUserCommand(bool DeleteExisting = false) : ICommand;