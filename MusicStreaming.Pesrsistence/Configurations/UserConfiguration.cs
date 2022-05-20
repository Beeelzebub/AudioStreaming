using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(e => e.ListeningHistory)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            builder.HasMany(e => e.Permissions)
                .WithMany(e => e.Users);

            builder.HasMany(e => e.FavoriteSongs)
                .WithMany(e => e.UsersWhoAddedToFavorite);

            builder.HasMany(e => e.FavoriteAlbums)
                .WithMany(e => e.UsersWhoAddedToFavorite);

            builder.HasMany(e => e.FavoritePlaylists)
                .WithMany(e => e.UsersWhoAddedToFavorite);
        }
    }
}
