using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class ReleaseConfiguration : IEntityTypeConfiguration<Release>
    {
        public void Configure(EntityTypeBuilder<Release> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Date)
                .IsRequired(false);

            builder.Property(e => e.Type)
                .IsRequired(true);

            builder.Property(e => e.Stage)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(150)
                .IsRequired(true);

            builder.Property(e => e.Title)
                .HasMaxLength(50)
                .IsRequired(true);

            builder.HasMany(e => e.UsersWhoAddedToFavorite)
                .WithMany(e => e.FavoriteReleases);

            builder.HasMany(e => e.Tracks)
                .WithOne(e => e.Release)
                .HasForeignKey(e => e.ReleaseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Participants)
                .WithOne(e => e.Release)
                .HasForeignKey(e => e.ReleaseId);


        }
    }
}
