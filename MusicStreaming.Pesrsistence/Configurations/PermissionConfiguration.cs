using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
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
