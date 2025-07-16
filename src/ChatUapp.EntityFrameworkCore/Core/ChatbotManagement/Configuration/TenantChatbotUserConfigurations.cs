using ChatUapp.Core.ChatbotManagement.AggregateRoots;
using ChatUapp.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.TenantManagement;

namespace ChatUapp.Core.ChatbotManagement.Configuration
{
    public class TenantChatbotUserConfigurations : IEntityTypeConfiguration<TenantChatbotUser>
    {
        public void Configure(EntityTypeBuilder<TenantChatbotUser> builder)
        {
            builder.ToTable(DbTableNameConsts.TenantChatbotUsers, DbSchemaNameConsts.Chatbot);

            builder.ConfigureByConvention();

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId).IsRequired(true);

            builder.Property(x => x.UserId).IsRequired(true);

            builder.Property(x => x.ChatbotId).IsRequired(true);

            builder.Property(x => x.Status).IsRequired(true);

            builder.HasOne<Chatbot>()
                .WithMany()
                .HasForeignKey(s => s.ChatbotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Tenant>()
               .WithMany()
               .HasForeignKey(s => s.TenantId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
