using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.ChatbotManagement.Consts;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ChatUapp.Core.ChatbotManagement.Configuration;

public class TrainingSourceConfiguration : IEntityTypeConfiguration<TrainingSource>
{
    public void Configure(EntityTypeBuilder<TrainingSource> builder)
    {
        builder.ToTable(DbTableNameConsts.TrainingSources, DbSchemaNameConsts.Chatbot);

        builder.ConfigureByConvention(); // Enables auditing, soft-delete, etc.

        builder.HasKey(ts => ts.Id);

        builder.Property(ts => ts.TenantId)
            .IsRequired();

        builder.Property(ts => ts.ChatbotId)
            .IsRequired();

        builder.Property(ts => ts.Name)
            .IsRequired()
            .HasMaxLength(TrainingSourceConsts.NameMaxLength);

        builder.Property(ts => ts.Description)
            .HasMaxLength(TrainingSourceConsts.DescriptionMaxLength);

        builder.HasOne<Chatbot>()
            .WithMany()
            .HasForeignKey(s => s.ChatbotId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🧠 Value Object: TrainingSourceOrigin
        builder.OwnsOne(ts => ts.Origin, origin =>
        {
            origin.Property(o => o.SourceType)
                .IsRequired()
                .HasColumnName(TrainingSourceConsts.OriginSourceTypeColumnName)
                .HasConversion<int>();

            origin.Property(o => o.SourceUrl)
                .HasMaxLength(TrainingSourceConsts.SourceUrlMaxLength)
                .HasColumnName(TrainingSourceConsts.OriginSourceUrlColumnName);

            origin.Property(o => o.FileName)
                .HasMaxLength(TrainingSourceConsts.FileNameMaxLength)
                .HasColumnName(TrainingSourceConsts.OriginFileNameColumnName);

            origin.Property(o => o.FileType)
                .HasMaxLength(TrainingSourceConsts.FileTypeMaxLength)
                .HasColumnName(TrainingSourceConsts.OriginFileTypeColumnName);

            origin.Property(o => o.TextContent)
                .HasColumnType(TrainingSourceConsts.OriginTextContentColumnType) // For large text content in PostgreSQL
                .HasColumnName(TrainingSourceConsts.OriginTextContentColumnName);

            origin.WithOwner(); // Optional in EF Core 5+, safe to include
        });

        builder.Property(ts => ts.LastUpdated)
            .IsRequired();

        // Optional index for performance
        builder.HasIndex(ts => ts.ChatbotId);
    }
}
