﻿using ChatUapp.Localization;
using Localization.Resources.AbpUi;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace ChatUapp;

[DependsOn(
   typeof(ChatUappApplicationContractsModule),
   typeof(AbpPermissionManagementHttpApiModule),
   typeof(AbpSettingManagementHttpApiModule),
   typeof(AbpAccountHttpApiModule),
   typeof(AbpIdentityHttpApiModule),
   typeof(AbpTenantManagementHttpApiModule),
   typeof(AbpFeatureManagementHttpApiModule)
   )]
public class ChatUappHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ChatUappResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
