using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.IsConfirmed)
                .IsRequired(true);

            builder.Property(e => e.Pseudonym)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(e => e.Country)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(e => e.ProfileImageUri)
                .IsRequired(false);

            builder.Property(e => e.Email)
                .IsRequired(false);

            builder.HasMany(e => e.ParticipatingInSongs)
                .WithOne(e => e.Artist)
                .HasForeignKey(e => e.ArtistId);

            builder.HasMany(e => e.ParticipatingInReleases)
                .WithOne(e => e.Artist)
                .HasForeignKey(e => e.ArtistId);

        }
    }
}
