using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(e => e.SongCoverUri)
                .IsRequired(false);

            builder.HasOne(e => e.Release)
                .WithMany(e => e.Songs)
                .HasForeignKey(e => e.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Genres)
                .WithMany(e => e.Songs);

            builder.HasMany(e => e.UsersWhoAddedToFavorite)
                .WithMany(e => e.FavoriteSongs);

            builder.HasMany(e => e.ListeningHistory)
                .WithOne(e => e.Song)
                .HasForeignKey(e => e.SongId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
