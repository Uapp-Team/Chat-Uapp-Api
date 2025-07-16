using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Consts;
using ChatUapp.Core.ChatbotManagement.VOs;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.ChatbotManagement.Configuration;

public class ChatbotConfiguration : IEntityTypeConfiguration<Chatbot>
{
    public void Configure(EntityTypeBuilder<Chatbot> builder)
    {
        builder.ToTable(DbTableNameConsts.ChatBots, DbSchemaNameConsts.Chatbot);

        builder.ConfigureByConvention();

        // Primary Key
        builder.HasKey(c => c.Id);

        // TenantId (multi-tenancy)
        builder.Property(c => c.TenantId)
            .IsRequired();

        // Name
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(ChatbotConsts.NameMaxLength);

        // Description (nullable)
        builder.Property(c => c.Description)
            .HasMaxLength(ChatbotConsts.DescriptionMaxLength);

        // Header
        builder.Property(c => c.Header)
            .IsRequired()
            .HasMaxLength(ChatbotConsts.HeaderMaxLength);

        // SubHeader
        builder.Property(c => c.SubHeader)
            .IsRequired()
            .HasMaxLength(ChatbotConsts.SubHeaderMaxLength);

        // UniqueKey
        builder.Property(c => c.UniqueKey)
            .IsRequired()
            .HasMaxLength(ChatbotConsts.UniqueKeyMaxLength);

        builder.HasIndex(c => c.UniqueKey)
            .IsUnique(); // Enforces uniqueness

        // BrandImageUrl (nullable)
        builder.Property(c => c.BrandImageName)
            .HasMaxLength(ChatbotConsts.BrandImageNameMaxLength);

        // Enums: Store as integers
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<int>();

        // 📌 Value Object: IconStyle (Owned Type)
        builder.OwnsOne(c => c.IconStyle, icon =>
        {
            icon.Property(i => i.IconName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName(nameof(IconStyle.IconName));

            icon.Property(i => i.IconColor)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName(nameof(IconStyle.IconColor));

            icon.WithOwner(); // Defines ownership to Chatbot
        });


        // You could also map events if you're using Domain Events, but EF doesn't persist events directly
    }
}
