using ChatUapp.DbEntities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatUapp.Configurations.Messages;

public class PublicMessageConfiguration : IEntityTypeConfiguration<PublicMessage>
{
    public void Configure(EntityTypeBuilder<PublicMessage> builder)
    {
        builder.Property(x => x.BrowserSessionKey)
           .HasMaxLength(256)
           .IsRequired();
    }
}
