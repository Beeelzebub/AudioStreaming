using MusicStreaming.Domain.Abstractions;

namespace MusicStreaming.Domain.Entities
{
    public class Song : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public int ReleaseId { get; set; }

        public string PathInStorage { get; set; } = default!;

        public Release Release { get; set; } = default!;

        public List<SongParticipant> Participants { get; set; } = default!;

        public List<Genre> Genres { get; set; } = default!;

        public List<User> UsersWhoAddedToFavorite { get; set; } = default!;

        public List<ListeningHistory> ListeningHistory { get; set; } = default!;

        public List<Playlist> Playlists { get; set; } = default!;
    }
}
