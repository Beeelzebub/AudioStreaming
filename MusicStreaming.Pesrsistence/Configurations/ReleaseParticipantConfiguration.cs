using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class ReleaseParticipantConfiguration : IEntityTypeConfiguration<ReleaseParticipant>
    {
        public void Configure(EntityTypeBuilder<ReleaseParticipant> builder)
        {
            builder.HasKey(e => new { e.ReleaseId, e.ArtistId });

            builder.Property(e => e.Order)
                .IsRequired(true);

            builder.HasOne(e => e.Artist)
                .WithMany(e => e.ParticipatingInReleases)
                .HasForeignKey(e => e.ArtistId);

            builder.HasOne(e => e.Release)
                .WithMany(e => e.Participants)
                .HasForeignKey(e => e.ReleaseId);
        }
    }
}
