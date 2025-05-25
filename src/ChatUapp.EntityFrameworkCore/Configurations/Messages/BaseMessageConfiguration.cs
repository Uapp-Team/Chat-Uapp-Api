using ChatUapp.Constants;
using ChatUapp.DbEntities.Messages;
using ChatUapp.DbEntities.Messages.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatUapp.Configurations.Messages;

public class BaseMessageConfiguration : IEntityTypeConfiguration<BaseMessage>
{
    public void Configure(EntityTypeBuilder<BaseMessage> builder)
    {
        builder.ToTable(DbTableNames.Messages, DbSchemaNames.Messaging);

        builder.HasKey(x => x.Id);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId).IsRequired(false);

        builder.Property(x => x.Text)
            .HasConversion(
                v => v.Value, // Store string in DB
                v => new MessageText(v) // Read as value object
            )
            .HasMaxLength(5000)
            .IsRequired();

        builder.Property(x => x.MessageType)
            .IsRequired();

        builder.Property(x => x.ChatBotId)
            .IsRequired(false);

        builder.Property(x => x.Ip)
            .HasMaxLength(128)
            .IsRequired(false);

        builder.HasDiscriminator<string>("MessageDiscriminator")
            .HasValue<Message>("User")
            .HasValue<PublicMessage>("Public");
    }
}
