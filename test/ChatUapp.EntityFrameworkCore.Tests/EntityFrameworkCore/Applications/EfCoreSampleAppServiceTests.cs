using ChatUapp.Samples;
using Xunit;

namespace ChatUapp.EntityFrameworkCore.Applications;

[Collection(ChatUappTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ChatUappEntityFrameworkCoreTestModule>
{

}
