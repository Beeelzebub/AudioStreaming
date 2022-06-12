using AudioStreaming.Domain.Abstractions;

namespace AudioStreaming.Domain.Entities
{
    public class Playlist : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; } = string.Empty;

        public string PlaylistCoverUri { get; set; } = default!;

        public bool IsPrivate { get; set; }

        public List<Track> Tracks { get; set; } = default!;

        public List<User> UsersWhoAddedToFavorite { get; set; } = default!;

        public List<PlaylistPermission> Permissions { get; set; } = default!;
    }
}
