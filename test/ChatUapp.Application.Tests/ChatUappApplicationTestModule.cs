using Volo.Abp.Modularity;

namespace ChatUapp;

[DependsOn(
    typeof(ChatUappApplicationModule),
    typeof(ChatUappDomainTestModule)
)]
public class ChatUappApplicationTestModule : AbpModule
{

}
