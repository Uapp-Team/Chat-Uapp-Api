using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ChatUapp.Data;

/* This is used if database provider does't define
 * IChatUappDbSchemaMigrator implementation.
 */
public class NullChatUappDbSchemaMigrator : IChatUappDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
