using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AudioStreaming.Domain.Entities;

namespace AudioStreaming.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.RefreshToken)
                .IsRequired(false);

            builder.Property(e => e.RefreshTokenExperation)
                .IsRequired(false);

            builder.HasMany(e => e.ListeningHistory)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            builder.HasMany(e => e.Permissions)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            builder.HasMany(e => e.FavoriteTracks)
                .WithMany(e => e.UsersWhoAddedToFavorite);

            builder.HasMany(e => e.FavoritePlaylists)
                .WithMany(e => e.UsersWhoAddedToFavorite);

            builder.HasMany(e => e.FavoriteReleases)
                .WithMany(e => e.UsersWhoAddedToFavorite);
        }
    }
}
