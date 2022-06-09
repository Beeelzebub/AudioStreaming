using AudioStreaming.Domain.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace AudioStreaming.Domain.Entities
{
    public class User : IdentityUser, IEntity
    {
        public List<ListeningHistory> ListeningHistory { get; set; } = default!;

        public List<PlaylistPermission> Permissions { get; set; } = default!;

        public List<Track> FavoriteTracks { get; set; } = default!;

        public List<Playlist> FavoritePlaylists { get; set; } = default!;

        public List<Release> FavoriteReleases { get; set; } = default!;
    }
}
