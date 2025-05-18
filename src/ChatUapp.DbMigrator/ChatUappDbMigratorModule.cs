using ChatUapp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ChatUapp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ChatUappEntityFrameworkCoreModule),
    typeof(ChatUappApplicationContractsModule)
)]
public class ChatUappDbMigratorModule : AbpModule
{
}
