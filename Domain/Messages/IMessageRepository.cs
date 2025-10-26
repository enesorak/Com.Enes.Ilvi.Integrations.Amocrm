using Domain.Abstractions;

namespace Domain.Messages;

public interface IMessageRepository : IRepository<Message, MessageId>
{
    Task<List<string>> GetAllIdsAsync();
}