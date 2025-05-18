using ChatUapp.Samples;
using Xunit;

namespace ChatUapp.EntityFrameworkCore.Domains;

[Collection(ChatUappTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ChatUappEntityFrameworkCoreTestModule>
{

}
