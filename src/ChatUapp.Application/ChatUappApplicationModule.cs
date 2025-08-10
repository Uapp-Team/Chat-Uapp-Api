using ChatUapp.Core.Message.Interfaces;
using ChatUapp.Core.PermissionManagement.DefinitionProviders;
using ChatUapp.Core.PermissionManagement.Definitions;
using ChatUapp.HttpClients;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Linq;
using System.Net.Http.Headers;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace ChatUapp;

[DependsOn(
    typeof(ChatUappDomainModule),
    typeof(ChatUappApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class ChatUappApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ChatUappApplicationModule>();
        });

        context.Services.AddRefitClient<IChatGPTApi>()
           .ConfigureHttpClient(c =>
           {
               c.BaseAddress = new Uri("https://api.openai.com"); // e.g., "https://api.openai.com"
               c.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("Bearer", configuration["ChatBotEngine:ChatGptAPIKey"]);
           })
           .AddPolicyHandler(PollyPolicies.GetRetryPolicy());

        context.Services.AddRefitClient<IChatBotEngineApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration["ChatBotEngine:BaseUrl"]))
            .AddPolicyHandler(PollyPolicies.GetRetryPolicy());

        context.Services.AddScoped<ChatbotPermissionDefinitionProvider, MyChatbotPermissionDefinitionProvider>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var providers = context.ServiceProvider.GetServices<ChatbotPermissionDefinitionProvider>().ToList();

        if (!providers.Any())
        {
            throw new InvalidOperationException("No ChatbotPermissionDefinitionProviders registered.");
        }

        ChatbotPermissionRegistry.Initialize(providers);
    }
}
