using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace ChatUapp.Pages;

[Collection(ChatUappTestConsts.CollectionDefinitionName)]
public class Index_Tests : ChatUappWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
