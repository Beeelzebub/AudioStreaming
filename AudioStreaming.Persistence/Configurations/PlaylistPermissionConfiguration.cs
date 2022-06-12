using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class PlaylistPermissionConfiguration : IEntityTypeConfiguration<PlaylistPermission>
    {
        public void Configure(EntityTypeBuilder<PlaylistPermission> builder)
        {
            builder.HasKey(e => new { e.PlaylistId, e.UserId, e.Type });

            builder.Property(e => e.Type)
                .IsRequired(true);

            builder.HasOne(e => e.Playlist)
                .WithMany(e => e.Permissions)
                .HasForeignKey(e => e.PlaylistId);

            builder.HasOne(e => e.User)
                .WithMany(e => e.Permissions)
                .HasForeignKey(e => e.UserId);
        }
    }
}
