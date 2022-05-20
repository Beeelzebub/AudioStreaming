using MusicStreaming.Domain.Abstractions;

namespace MusicStreaming.Domain.Entities
{
    public class Playlist : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public string PlaylistCoverUri { get; set; } = default!;

        public bool IsPrivate { get; set; }

        public int OwnerId { get; set; }

        public User? Owner { get; set; } = default;

        public List<Song> Songs { get; set; } = default!;

        public List<User> UsersWhoAddedToFavorite { get; set; } = default!;

        public List<Permission> Permissions { get; set; } = default!;
    }
}
