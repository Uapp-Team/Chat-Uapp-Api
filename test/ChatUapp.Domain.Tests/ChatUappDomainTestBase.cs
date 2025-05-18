using Volo.Abp.Modularity;

namespace ChatUapp;

/* Inherit from this class for your domain layer tests. */
public abstract class ChatUappDomainTestBase<TStartupModule> : ChatUappTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
