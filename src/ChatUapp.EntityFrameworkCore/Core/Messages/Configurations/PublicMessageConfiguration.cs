using ChatUapp.Constants;
using ChatUapp.Core.Messages.Messages.VO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.Messages.Configurations;

public class PublicMessageConfiguration : IEntityTypeConfiguration<PublicMessage>
{
    public void Configure(EntityTypeBuilder<PublicMessage> builder)
    {
        builder.ToTable(DbTableNames.Messages, DbSchemaNames.Messaging);
        builder.ConfigureByConvention();
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

        builder.Property(x => x.BrowserSessionKey)
            .IsRequired(true);

        builder.Property(x => x.Ip)
            .HasMaxLength(128)
            .IsRequired(false);
    }
}
