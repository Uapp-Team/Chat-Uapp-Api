using ChatUapp.Core.Interfaces.Emailing;
using ChatUapp.Infrastructure.Emailing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace ChatUapp.Infrastructure
{
    [DependsOn(
    typeof(AbpEmailingModule)
    )]
    public class ChatUappInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Also register your custom interface
            context.Services.Replace(ServiceDescriptor.Singleton<IAppEmailSender, AppEmailSender>());

        }
    }
}
