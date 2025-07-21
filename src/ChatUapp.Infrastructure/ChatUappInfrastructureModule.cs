using ChatUapp.Core.Interfaces;
using ChatUapp.Core.Interfaces.Chatbot;
using ChatUapp.Core.Interfaces.Emailing;
using ChatUapp.Core.Interfaces.FileStorage;
using ChatUapp.Infrastructure.Emailing;
using ChatUapp.Infrastructure.FileStorage;
using ChatUapp.Infrastructure.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Azure;
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
            var configuration = context.Services.GetConfiguration();
            // Also register your custom interface
            context.Services.Replace(ServiceDescriptor.Singleton<IAppEmailSender, AppEmailSender>());
            context.Services.AddTransient<IBlobStorageService, BlobStorageService>();
            context.Services.AddScoped<IDomainGuidGenerator, DomainGuidGenerator>();
            context.Services.AddScoped<IAskMessageService, AskMessageService>();

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseAzure(azure =>
                    {
                        azure.ConnectionString = configuration["AzureBlobStorage:ConnectionString"];
                        azure.ContainerName = configuration["AzureBlobStorage:ContainerName"];
                        azure.CreateContainerIfNotExists = true;
                    });
                    container.IsMultiTenant = true;
                });
            });
        }
    }
}
