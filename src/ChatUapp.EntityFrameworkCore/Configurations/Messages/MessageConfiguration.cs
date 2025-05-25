using ChatUapp.DbEntities.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatUapp.Configurations.Messages;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(x => x.UserId).IsRequired();

        builder.Property(x => x.SessionId).IsRequired();

        builder.Property(x => x.IsLike).IsRequired();
    }
}
