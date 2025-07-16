using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Consts;
using ChatUapp.Core.ChatbotManagement.Entities;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.TenantManagement;

namespace ChatUapp.Core.ChatbotManagement.Configuration;

public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
{
    public void Configure(EntityTypeBuilder<ChatSession> builder)
    {
        builder.ToTable(DbTableNameConsts.ChatSessions, DbSchemaNameConsts.Chatbot);

        builder.ConfigureByConvention();

        builder.HasKey(s => s.Id);

        builder.Property(s => s.TenantId).IsRequired();

        builder.Property(s => s.SessionCreator).IsRequired();
        builder.Property(s => s.ChatbotId).IsRequired();

        builder.Property(s => s.Title)
            .HasMaxLength(ChatSessionConsts.TitleMaxLength);

        builder.Property(s => s.Ip)
            .HasMaxLength(ChatSessionConsts.IpMaxLength);

        builder.Property(s => s.BrowserSessionKey)
            .HasMaxLength(ChatSessionConsts.BrowserSessionKeyMaxLength);

        builder.HasOne<Chatbot>()
            .WithMany()
            .HasForeignKey(s => s.ChatbotId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Messages)
            .WithOne()
            .HasForeignKey(x => x.SessionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Optional indexes (recommended)
        builder.HasIndex(s => s.SessionCreator);
        builder.HasIndex(s => s.ChatbotId);
    }
}
