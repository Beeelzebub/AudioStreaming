using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class TrackParticipantConfiguration : IEntityTypeConfiguration<TrackParticipant>
    {
        public void Configure(EntityTypeBuilder<TrackParticipant> builder)
        {
            builder.HasKey(e => new { e.TrackId, e.ArtistId });

            builder.Property(e => e.Order)
                .IsRequired(true);

            builder.Property(e => e.Role)
                .IsRequired(true);

            builder.HasOne(e => e.Artist)
                .WithMany(e => e.ParticipatingInTracks)
                .HasForeignKey(e => e.ArtistId);

            builder.HasOne(e => e.Track)
                .WithMany(e => e.Participants)
                .HasForeignKey(e => e.TrackId);
        }
    }
}