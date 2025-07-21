using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ChatUapp.Core.ChatbotManagement;

public class ChatSessionRepository : EfCoreRepository<ChatUappDbContext, ChatSession, Guid>, IChatSessionRepository
{
    public ChatSessionRepository(IDbContextProvider<ChatUappDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
