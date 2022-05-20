using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class SongParticipantConfiguration : IEntityTypeConfiguration<SongParticipant>
    {
        public void Configure(EntityTypeBuilder<SongParticipant> builder)
        {
            builder.HasKey(e => new { e.SongId, e.ArtistId });

            builder.Property(e => e.Order)
                .IsRequired(true);

            builder.Property(e => e.Role)
                .IsRequired(true);

            builder.HasOne(e => e.Artist)
                .WithMany(e => e.ParticipatingInSongs)
                .HasForeignKey(e => e.ArtistId);

            builder.HasOne(e => e.Song)
                .WithMany(e => e.Participants)
                .HasForeignKey(e => e.SongId);
        }
    }
}