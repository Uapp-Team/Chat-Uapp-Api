using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using Volo.Abp.Domain.Repositories;

namespace ChatUapp.Core.ChatbotManagement.Sessions;

public interface IChatSessionRepository : IRepository<ChatSession>
{
}
