using ChatUapp;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("ChatUapp.Web.csproj");
await builder.RunAbpModuleAsync<ChatUappWebTestModule>(applicationName: "ChatUapp.Web");

public partial class Program
{
}
