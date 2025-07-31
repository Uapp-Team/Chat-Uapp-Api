using ChatUapp.Core.ChatbotManagement.DTOs.Chatbot;
using ChatUapp.Core.ChatbotManagement.Enums;
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
using Volo.Abp.Users;

namespace ChatUapp.Core.ChatbotManagement.QueryServices;

public class UserChatSummaryQueryService : IUserChatSummaryQueryService, ITransientDependency
{
    private readonly ChatUappDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICurrentUser _currentUser;

    public UserChatSummaryQueryService(
        ChatUappDbContext dbContext,
        UserManager<IdentityUser> userManager,
        ICurrentUser currentUser)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _currentUser = currentUser;
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


        var filteredUserQuery = _dbContext.Users.AsNoTracking().WhereIf(
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

    public async Task<UserDashboardSummaryDto> GetUserDashboardSummariesAsync(
        DateTime startDate, DateTime endDate, Guid? chatbotId)
    {
        // Normalize dates to cover the full days
        var normalizedStartDate = startDate.Date; // 00:00:00
        var normalizedEndDate = endDate.Date.AddDays(1); // exclusive upper bound

        var userBotIds = await _dbContext.TenantChatbotUsers
             .AsNoTracking()
             .Where(x => x.UserId == _currentUser.Id)
             .WhereIf(chatbotId.HasValue, x => x.ChatbotId == chatbotId)
             .Select(x => x.ChatbotId)
             .ToListAsync();

        // 1. Total messages (within date range)
        var totalMessagesCount = await _dbContext.ChatSessions
            .Where(x => userBotIds.Contains(x.ChatbotId))
            .SelectMany(s => s.Messages)
            .Where(m => m.SentAt >= normalizedStartDate && m.SentAt < normalizedEndDate)
            .CountAsync();

        // 2. Active chat bots (regardless of date range)
        var activeChatbotsCount = await _dbContext.Chatbots
            .Where(b => userBotIds.Contains(b.Id) && b.Status == ChatbotStatus.Active)
            .CountAsync();

        // 3. Total users (within date range)
        var totalUsersCount = await _dbContext.ChatSessions.AsNoTracking()
            .Where(x => userBotIds.Contains(x.ChatbotId) && x.CreationTime >= normalizedStartDate && x.CreationTime < normalizedEndDate)
            .Select(u => u.CreatorId)
            .Distinct()
            .CountAsync();

        // 4. Satisfaction rate
        var botPerformance = await _dbContext.ChatSessions
            .Where(x => userBotIds.Contains(x.ChatbotId) && x.CreationTime >= normalizedStartDate && x.CreationTime < normalizedEndDate)
            .Select(s => new
            {
                BotId = s.ChatbotId,
                BotName = _dbContext.Chatbots.Where(b => b.Id == s.ChatbotId).Select(b => b.Name).FirstOrDefault(),
                SatisfactionRate = s.Messages.Count(m => m.ReactType == ReactType.Like) * 100.0 /
                                   (s.Messages.Count(m => m.ReactType == ReactType.Like || m.ReactType == ReactType.Dislike) == 0
                                       ? 1
                                       : s.Messages.Count(m => m.ReactType == ReactType.Like || m.ReactType == ReactType.Dislike))
            })
            .GroupBy(x => new { x.BotId, x.BotName })
            .Select(g => new
            {
                BotId = g.Key.BotId,
                BotName = g.Key.BotName,
                SatisfactionRate = g.Average(x => x.SatisfactionRate)
            })
            .ToListAsync();

        return new UserDashboardSummaryDto
        {
            TotalMessagesCount = totalMessagesCount,
            ActiveChatbotsCount = activeChatbotsCount,
            TotalUsersCount = totalUsersCount,
            BotPerformance = botPerformance.Select(bp => new BotPerformanceDto
            {
                BotId= bp.BotId,
                BotName = bp.BotName!,
                SatisfactionRate = Math.Round(bp.SatisfactionRate, 2)
            }).ToList()
        };
    }
}
