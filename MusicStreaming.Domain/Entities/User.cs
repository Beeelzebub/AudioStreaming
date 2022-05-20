using MusicStreaming.Domain.Abstractions;

namespace MusicStreaming.Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public List<ListeningHistory> ListeningHistory { get; set; } = default!;

        public List<Permission> Permissions { get; set; } = default!;

        public List<Song> FavoriteSongs { get; set; } = default!;

        public List<Playlist> FavoritePlaylists { get; set; } = default!;

        public List<Release> FavoriteReleases { get; set; } = default!;
    }
}
