﻿using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace ChatUapp.EntityFrameworkCore;

public static class ChatUappEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        ChatUappGlobalFeatureConfigurator.Configure();
        ChatUappModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
            .MapEfCoreProperty<IdentityUser, string>(
                "TitlePrefix",
                (entityBuilder, propertyBuilder) =>
                {
                    propertyBuilder.HasMaxLength(10);
                }
            );

            ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, string>(
                    "FacebookUrl",
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(255);
                    }
                );

            ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, string>(
                    "InstagramUrl",
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(255);
                    }
                );

            ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, string>(
                    "LinkedInUrl",
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(255);
                    }
                );

            ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, string>(
                    "TwitterUrl",
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(255);
                    }
                );
            /* You can configure extra properties for the
             * entities defined in the modules used by your application.
             *
             * This class can be used to map these extra properties to table fields in the database.
             *
             * USE THIS CLASS ONLY TO CONFIGURE EF CORE RELATED MAPPING.
             * USE ChatUappModuleExtensionConfigurator CLASS (in the Domain.Shared project)
             * FOR A HIGH LEVEL API TO DEFINE EXTRA PROPERTIES TO ENTITIES OF THE USED MODULES
             *
             * Example: Map a property to a table field:

                 ObjectExtensionManager.Instance
                     .MapEfCoreProperty<IdentityUser, string>(
                         "MyProperty",
                         (entityBuilder, propertyBuilder) =>
                         {
                             propertyBuilder.HasMaxLength(128);
                         }
                     );

             * See the documentation for more:
             * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
             */
        });
    }
}
