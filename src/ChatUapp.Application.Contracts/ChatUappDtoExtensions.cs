using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace ChatUapp;

public static class ChatUappDtoExtensions
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            // Extend module DTOs (no need to create new DTOs!)
            ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                new[]
                {
                    typeof(IdentityUserDto),
                    typeof(IdentityUserUpdateDto),
                    typeof(IdentityUserCreateDto)
                },
                "TitlePrefix"
            );

            ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                new[]
                {
                    typeof(IdentityUserDto),
                    typeof(IdentityUserUpdateDto),
                    typeof(IdentityUserCreateDto)
                },
                "InstagramUrl"
            );

            ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                new[]
                {
                    typeof(IdentityUserDto),
                    typeof(IdentityUserUpdateDto),
                    typeof(IdentityUserCreateDto)
                },
                "LinkedInUrl"
            );

            ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                new[]
                {
                    typeof(IdentityUserDto),
                    typeof(IdentityUserUpdateDto),
                    typeof(IdentityUserCreateDto)
                },
                "TwitterUrl"
            );

            ObjectExtensionManager.Instance.AddOrUpdateProperty<string>(
                new[]
                {
                    typeof(IdentityUserDto),
                    typeof(IdentityUserUpdateDto),
                    typeof(IdentityUserCreateDto)
                },
                "FacebookUrl"
            );
       
            /* You can add extension properties to DTOs
             * defined in the depended modules.
             *
             * Example:
             *
             * ObjectExtensionManager.Instance
             *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
             *
             * See the documentation for more:
             * https://docs.abp.io/en/abp/latest/Object-Extensions
             */
        });
    }
}
