using AudioStreaming.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioStreaming.Persistence.Configurations
{
    public class ReleaseVerificationHistoryConfiguration : IEntityTypeConfiguration<ReleaseVerificationHistory>
    {
        public void Configure(EntityTypeBuilder<ReleaseVerificationHistory> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Date)
                .IsRequired(true);

            builder.Property(e => e.Comment)
                .IsRequired(false);

            builder.Property(e => e.NewStage)
                .IsRequired(true);

            builder.HasOne(e => e.Release)
                .WithMany(e => e.VerificationHistory)
                .HasForeignKey(e => e.ReleaseId);
        }
    }
}
