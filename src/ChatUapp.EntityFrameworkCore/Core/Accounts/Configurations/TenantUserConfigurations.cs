using ChatUapp.Constants;
using ChatUapp.Core.Accounts.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.Accounts.Configurations
{
    public class TenantUserConfigurations : IEntityTypeConfiguration<TenantUser>
    {
        public void Configure(EntityTypeBuilder<TenantUser> builder)
        {
            builder.ToTable(DbTableNames.TenantUsers, DbSchemaNames.Tenant);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId).IsRequired(true);

            builder.Property(x => x.UserId).IsRequired(true);

            builder.Property(x => x.IsDeleted).IsRequired(true);

        }
    }
}
