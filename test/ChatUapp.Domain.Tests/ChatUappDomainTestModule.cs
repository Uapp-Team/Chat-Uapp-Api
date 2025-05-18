using Volo.Abp.Modularity;

namespace ChatUapp;

[DependsOn(
    typeof(ChatUappDomainModule),
    typeof(ChatUappTestBaseModule)
)]
public class ChatUappDomainTestModule : AbpModule
{

}
