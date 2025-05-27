using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChatUapp.Data;
using Volo.Abp.DependencyInjection;

namespace ChatUapp.EntityFrameworkCore;

public class EntityFrameworkCoreChatUappDbSchemaMigrator
    : IChatUappDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreChatUappDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ChatUappDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

         await _serviceProvider
            .GetRequiredService<ChatUappDbContext>()
            .Database
            .MigrateAsync();
    }
}
