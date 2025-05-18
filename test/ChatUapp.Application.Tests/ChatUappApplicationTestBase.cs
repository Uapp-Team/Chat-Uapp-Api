using Volo.Abp.Modularity;

namespace ChatUapp;

public abstract class ChatUappApplicationTestBase<TStartupModule> : ChatUappTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
