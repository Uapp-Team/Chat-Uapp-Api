using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace ChatUapp.Core.ChatUAppDbSeedContributor;

public class BotDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Chatbot, Guid> _botRepository;
    private readonly IdentityUserManager _userManager;
    private readonly IdentityRoleManager _roleManager;
    private readonly ChatbotManager _chatbotManager;
    private readonly ICurrentTenant _currentTenant;
    private readonly ITenantRepository _tenantRepository;


    public BotDataSeedContributor(
        IRepository<Chatbot, Guid> botRepository,
        ICurrentTenant currentTenant,
        ITenantRepository tenantRepository,
        ChatbotManager chatbotManager
,
        IdentityUserManager userManager,
        IdentityRoleManager roleManager)
    {
        _botRepository = botRepository;
        _currentTenant = currentTenant;
        _tenantRepository = tenantRepository;
        _chatbotManager = chatbotManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Manually get the known tenant (you may want to use FindByNameAsync if not hardcoding ID)
        var tenant = await _tenantRepository.GetAsync(Guid.Parse("3a1b2a14-1f7e-d0bf-7aac-5a1ddff49d80"));
        //var tenant = await _tenantRepository.FindByNameAsync("Default");

        using (_currentTenant.Change(tenant.Id))
        {
            // Don't insert if chatbot with name "Default" already exists
            var existingBots = await _botRepository.GetListAsync();

            if (existingBots.Any(b => b.isDefalt == true))
            {
                return;
            }

            var bot = await _chatbotManager.SeedAsync(
                "ChatUapp",
                "ChatUapp Header",
                "ChatUapp Dub Header",
                "ChatUapp Icon Name",
                "ChatUapp",
                tenant.Id
                );

            _chatbotManager.SetDefault(bot);

            await _botRepository.InsertAsync(bot, autoSave: true);
        }
    }
}
