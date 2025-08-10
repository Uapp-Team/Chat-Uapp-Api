using Azure.Storage.Blobs;
using ChatUapp.Core.Interfaces.Chatbot;
using ChatUapp.Core.Interfaces.Emailing;
using ChatUapp.Core.Interfaces.FileStorage;
using ChatUapp.Core.Thirdparty.Interfaces;
using ChatUapp.Infrastructure.BotEngineServices;
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
    typeof(AbpEmailingModule),
    typeof(AbpBlobStoringAzureModule)
    )]
    public class ChatUappInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            // Register BlobServiceClient as singleton so it can be injected
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("Azure Blob Storage connection string is not configured.");
            }

            context.Services.AddSingleton(x => new BlobServiceClient(connectionString));

            // Register your services
            context.Services.Replace(ServiceDescriptor.Singleton<IAppEmailSender, AppEmailSender>());
            context.Services.AddTransient<IBlobStorageService, BlobStorageService>();
            context.Services.AddScoped<IDomainGuidGenerator, DomainGuidGenerator>();
            context.Services.AddScoped<IBotEngineManageService, BotEngineManageService>();

            // Configure ABP BlobStoring Azure provider
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseAzure(azure =>
                    {
                        azure.ConnectionString = connectionString;
                        azure.ContainerName = configuration["AzureBlobStorage:ContainerName"];
                        azure.CreateContainerIfNotExists = true;
                    });
                    container.IsMultiTenant = true;
                });
            });
        }

    }
}
