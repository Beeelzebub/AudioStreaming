using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder.HasKey(e => e.Id);


            builder.HasOne(e => e.Release)
                .WithMany(e => e.Tracks)
                .HasForeignKey(e => e.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Genres)
                .WithMany(e => e.Tracks);

            builder.HasMany(e => e.UsersWhoAddedToFavorite)
                .WithMany(e => e.FavoriteTracks);

            builder.HasMany(e => e.ListeningHistory)
                .WithOne(e => e.Track)
                .HasForeignKey(e => e.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
