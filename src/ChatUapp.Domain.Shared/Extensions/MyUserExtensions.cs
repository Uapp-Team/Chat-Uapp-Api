using Volo.Abp.ObjectExtending;

namespace ChatUapp.Extensions
{
    public static class MyUserExtensions
    {
        public static void Configure()
        {
            ObjectExtensionManager.Instance.Modules()
                .ConfigureIdentity(identity =>
                {
                    identity.ConfigureUser(user =>
                    {
                        user.AddOrUpdateProperty<string>("TitlePrefix");
                    });
                });
        }
    }
}
