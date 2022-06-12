using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.Property(e => e.PlaylistCoverUri)
                .IsRequired(false);

            builder.Property(e => e.IsPrivate)
                .IsRequired(true);

            builder.HasMany(e => e.Permissions)
                .WithOne(e => e.Playlist)
                .HasForeignKey(e => e.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Tracks)
                .WithMany(e => e.Playlists);
        }
    }
}
