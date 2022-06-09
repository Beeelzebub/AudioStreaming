using AudioStreaming.Domain.Abstractions;

namespace AudioStreaming.Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }

        public List<ListeningHistory> ListeningHistory { get; set; } = default!;

        public List<PlaylistPermission> Permissions { get; set; } = default!;

        public List<Track> FavoriteTracks { get; set; } = default!;

        public List<Playlist> FavoritePlaylists { get; set; } = default!;

        public List<Release> FavoriteReleases { get; set; } = default!;
    }
}
