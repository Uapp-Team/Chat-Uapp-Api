using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;
using ChatUapp.Message.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ChatUapp.HttpClients;
using Refit;
using System;
using System.Net.Http.Headers;
using Volo.Abp.MailKit;
using Volo.Abp.Emailing;

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
    typeof(AbpEmailingModule),
    typeof(AbpMailKitModule)
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

       
    }
}
