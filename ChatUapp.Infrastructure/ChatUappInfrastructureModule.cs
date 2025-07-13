using ChatUapp.Core.Interfaces.Emailing;
using ChatUapp.Infrastructure.Emailing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace ChatUapp.Infrastructure
{

    public class ChatUappInfrastructureModule : AbpModule
    {


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Replace IEmailSender with your custom implementation
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, AppEmailSender>());

            // Also register your custom interface
            context.Services.Replace(ServiceDescriptor.Singleton<IAppEmailSender, AppEmailSender>());

        }
    }




}
