using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Enums;
using ChatUapp.Core.ChatbotManagement.VOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace ChatUapp.Core.ChatbotManagement.Services;

public class BotDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Chatbot, Guid> _botRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ITenantRepository _tenantRepository;

    public BotDataSeedContributor(
        IRepository<Chatbot, Guid> botRepository,
        ICurrentTenant currentTenant,
        IGuidGenerator guidGenerator,
        ITenantRepository tenantRepository)
    {
        _botRepository = botRepository;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _tenantRepository = tenantRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Manually get the known tenant (you may want to use FindByNameAsync if not hardcoding ID)
        var tenant = await _tenantRepository.GetAsync(Guid.Parse("3a1b2a14-1f7e-d0bf-7aac-5a1ddff49d80"));

        using (_currentTenant.Change(tenant.Id))
        {
            // Don't insert if chatbot with name "Defolt" already exists
            var existingBots = await _botRepository.GetListAsync();

            if (existingBots.Any(b => b.Name == "ChatUapp"))
            {
                return;
            }

            var bot = new Chatbot(
                _guidGenerator.Create(),
                "ChatUapp",
                "ChatUapp Header",
                "ChatUapp Dub Header",
                _guidGenerator.Create().ToString(),
                new IconStyle("ChatUapp Icon Name", "ChatUapp"),
                ChatbotStatus.Draft,
                tenant.Id
            );

            bot.SetDefault();

            await _botRepository.InsertAsync(bot, autoSave: true);
        }
    }
}
