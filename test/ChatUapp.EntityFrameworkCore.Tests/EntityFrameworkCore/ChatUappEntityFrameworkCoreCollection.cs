using Xunit;

namespace ChatUapp.EntityFrameworkCore;

[CollectionDefinition(ChatUappTestConsts.CollectionDefinitionName)]
public class ChatUappEntityFrameworkCoreCollection : ICollectionFixture<ChatUappEntityFrameworkCoreFixture>
{

}
