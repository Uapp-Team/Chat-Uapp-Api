using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;

namespace ChatUapp
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
