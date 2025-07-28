using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using ChatUapp.Core.ChatbotManagement.Interfaces;
using ChatUapp.Core.Extensions;
using ChatUapp.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ChatUapp.Core.ChatbotManagement.QueryServices;

public class UserChatSummaryQueryService : IUserChatSummaryQueryService,ITransientDependency
{
    private readonly ChatUappDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public UserChatSummaryQueryService(
        ChatUappDbContext dbContext,
        UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<PagedResultDto<GetAllChatDto>> GetUserChatSummariesAsync(GetAllChatFilterDto filter)
    {
        var sessionQuery = _dbContext.ChatSessions
            .AsNoTracking();

        // Apply filters
        var filteredSessionQuery = sessionQuery
            .WhereIf(filter.ChatbotId.HasValue, x => x.ChatbotId == filter.ChatbotId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Country), x => x.LocationSnapshot.CountryName == filter.Country)
            .WhereIf(filter.CreatedAfter.HasValue, x => x.CreationTime >= filter.CreatedAfter!.Value)
            .WhereIf(filter.CreatedBefore.HasValue, x => x.CreationTime <= filter.CreatedBefore!.Value);


        var filteredUserQuery =  _dbContext.Users.WhereIf(
            !string.IsNullOrWhiteSpace(filter.Text), 
            x => x.Name.Contains(filter.Text!) || x.Email.Contains(filter.Text!));


        // Step 1: Join filtered sessions with filtered users
        var query =
            from session in filteredSessionQuery
            join user in filteredUserQuery on session.SessionCreator equals user.Id
            select new { session, user };

        // Step 2: Group by UserId
        var grouped = query
            .GroupBy(x => x.user.Id)
            .Select(g => new
            {
                User = g.First().user,
                FirstSession = g.OrderBy(x => x.session.CreationTime).Select(x => x.session).FirstOrDefault(),
                SessionCount = g.Count(),
                LastMessageTime = g.SelectMany(x => x.session.Messages)
                                   .OrderByDescending(m => m.SentAt)
                                   .Select(m => m.SentAt)
                                   .FirstOrDefault()
            });

        // Step 3: Get total count before paging
        var totalCount = await grouped.CountAsync();

        // Step 4: Apply sorting and paging
        var paged = await grouped
            .OrderByDescending(x => x.LastMessageTime)
            .Skip(filter.SkipCount)
            .Take(filter.MaxResultCount)
            .ToListAsync();

        // Step 5: Load required chatbot data (batch fetch)
        var botIds = paged.Select(x => x.FirstSession.ChatbotId).Distinct().ToList();
        var botDict = await _dbContext.Chatbots
            .Where(b => botIds.Contains(b.Id))
            .Select(b => new { b.Id, b.Name })
            .ToDictionaryAsync(b => b.Id, b => b.Name);

        // Step 6: Final projection
        var result = paged.Select(x =>
        {
            var botName = botDict.GetValueOrDefault(x.FirstSession.ChatbotId) ?? string.Empty;

            return new GetAllChatDto
            {
                UserId = x.User.Id,
                UserName = x.User.UserName ?? string.Empty,
                Email = x.User.Email ?? string.Empty,
                Country = x.FirstSession.LocationSnapshot.CountryName,
                Flag = x.FirstSession.LocationSnapshot.Flag,
                BotId = x.FirstSession.ChatbotId,
                BotName = botName,
                SessionCount = x.SessionCount,
                LastMessage = x.LastMessageTime.ToFriendlyDate() ?? "invalid"
            };
        }).ToList();

        // Step 7: Return as paged result
        return new PagedResultDto<GetAllChatDto>(totalCount, result);
    }
}
