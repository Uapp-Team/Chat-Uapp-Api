using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.Accounts.Seed;

public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentTenant _currentTenant;

    public RoleDataSeedContributor(
        IdentityRoleManager roleManager,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
    {
        _roleManager = roleManager;
        _guidGenerator = guidGenerator;
        _currentTenant = currentTenant;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateRoleAsync("enduser");
        await CreateRoleAsync("chatbotuser");
    }

    private async Task CreateRoleAsync(string roleName)
    {
        if (await _roleManager.FindByNameAsync(roleName) == null)
        {
            var role = new IdentityRole(_guidGenerator.Create(), roleName , _currentTenant.Id);
            await _roleManager.CreateAsync(role);
        }
    }
}
