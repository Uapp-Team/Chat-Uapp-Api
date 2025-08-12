using ChatUapp.Core.Constants;
using ChatUapp.Core.PermisionManagement.Consts;
using ChatUapp.Core.PermissionManagement.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.PermissionManagement.Configurations;

public class ChatbotUserPermissionConfiguraton : IEntityTypeConfiguration<ChatbotUserPermission>
{
    public void Configure(EntityTypeBuilder<ChatbotUserPermission> builder)
    {
        builder.ToTable(DbTableNameConsts.ChatbotUserPermissions, DbSchemaNameConsts.Chatbot);

        builder.ConfigureByConvention();

        // Primary Key
        builder.HasKey(c => c.Id);

        // TenantId (multi-tenancy)
        builder.Property(c => c.TenantId)
            .IsRequired();

        // Name
        builder.Property(c => c.PermissionName)
            .IsRequired()
            .HasMaxLength(PermissionConsts.PermissionNameMaxLength);
    }
}
