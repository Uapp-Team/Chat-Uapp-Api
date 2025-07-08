using ChatUapp.Core.Accounts.Entitys;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.Accounts.Configurations
{
    public class TenantChatbotUserConfigurations : IEntityTypeConfiguration<TenantChatbotUser>
    {
        public void Configure(EntityTypeBuilder<TenantChatbotUser> builder)
        {
            builder.ToTable(DbTableNameConsts.TenantChatbotUsers, DbSchemaNameConsts.Tenant);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId).IsRequired(true);

            builder.Property(x => x.UserId).IsRequired(true);

            builder.Property(x => x.ChatbotId).IsRequired(true);

            builder.Property(x => x.Status).IsRequired(true);
        }
    }
}
