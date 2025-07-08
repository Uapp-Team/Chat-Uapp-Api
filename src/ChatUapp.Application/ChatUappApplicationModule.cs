using ChatUapp.Core.Emailing;
using ChatUapp.Core.Emailing.Interfaces;
using ChatUapp.Core.Message.Interfaces;
using ChatUapp.HttpClients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http.Headers;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Smtp;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.MailKit;
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
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpEmailingModule)
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

        context.Services.Replace(ServiceDescriptor.Singleton<IAppEmailSender, AppEmailSender>());

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

       
    }
}
