using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Consts;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.ChatbotManagement.Configuration;

public class BotInvitationConfiguration : IEntityTypeConfiguration<BotInvitation>
{
    public void Configure(EntityTypeBuilder<BotInvitation> builder)
    {
        builder.ToTable(DbTableNameConsts.BotInvitations, DbSchemaNameConsts.Chatbot);

        builder.ConfigureByConvention();

        // Primary Key
        builder.HasKey(c => c.Id);

        //  BotId
        builder.Property(x => x.BotId)
            .IsRequired();

        //  TenantId
        builder.Property(x => x.TenantId)
            .IsRequired();

        //  InvitedBy
        builder.Property(x => x.InvitedBy)
            .IsRequired();

        //  UserEmail
        builder.Property(x => x.UserEmail)
            .IsRequired()
            .HasMaxLength(BotInvitationConst.EmailMaxLength);

        // Role
        builder.Property(x => x.Role)
            .IsRequired()
            .HasMaxLength(BotInvitationConst.RoleMaxLength);

        // IsRegistered
        builder.Property(x => x.IsRegistered)
            .IsRequired();

        // InvitationToken
        builder.Property(x => x.InvitationToken)
            .IsRequired()
            .HasMaxLength(BotInvitationConst.InvitationTokenMaxLength);
    }
}
