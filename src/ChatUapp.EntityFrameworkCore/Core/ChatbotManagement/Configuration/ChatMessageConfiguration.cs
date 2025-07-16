using ChatUapp.Core.ChatbotManagement.Consts;
using ChatUapp.Core.ChatbotManagement.Entities;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.ChatbotManagement.Configuration;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable(DbTableNameConsts.ChatMessages, DbSchemaNameConsts.Chatbot);

        builder.ConfigureByConvention(); // ABP: Adds auditing, soft delete, etc.

        builder.HasKey(m => m.Id);

        builder.Property(m => m.SessionId)
            .IsRequired();

        builder.OwnsOne(m => m.Role, rb =>
        {
            rb.Property(r => r.Value)
              .IsRequired()
              .HasMaxLength(ChatSessionConsts.MessageRoleMaxLength)
              .HasColumnName(ChatSessionConsts.MessageRoleColumnName);
        });

        builder.Property(m => m.Type)
            .IsRequired()
            .HasConversion<int>();

        // ✅ Configure Value Object: MessageText (stored as TEXT in PostgreSQL)
        builder.OwnsOne(m => m.Content, cb =>
        {
            cb.Property(c => c.Value)
              .IsRequired()
              .HasColumnName(ChatSessionConsts.MessageColumnName) // e.g., "Content"
              .HasColumnType(ChatSessionConsts.MessageColumnType); // e.g., "text"

            // No need for WithOwner() unless there's navigation
        });

        builder.Property(m => m.SentAt)
            .IsRequired();

        builder.HasIndex(m => m.SentAt);
    }
}
