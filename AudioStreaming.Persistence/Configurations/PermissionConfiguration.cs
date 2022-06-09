using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<PlaylistPermission>
    {
        public void Configure(EntityTypeBuilder<PlaylistPermission> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Type)
                .IsRequired(true);

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasOne(e => e.Playlist)
                .WithMany(e => e.Permissions)
                .HasForeignKey(e => e.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Users)
                .WithMany(e => e.Permissions);
        }
    }
}
