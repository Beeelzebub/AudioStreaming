using AudioStreaming.Domain.Abstractions;

namespace AudioStreaming.Domain.Entities
{
    public class Track : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public int ReleaseId { get; set; }

        public int? PositionInChart { get; set; }

        public string PathInStorage { get; set; } = default!;

        public Release Release { get; set; } = default!;

        public List<TrackParticipant> Participants { get; set; } = new();

        public List<Artist> Artists { get; set; } = new ();

        public List<Genre> Genres { get; set; } = new ();

        public List<User> UsersWhoAddedToFavorite { get; set; } = new ();

        public List<ListeningHistory> ListeningHistory { get; set; } = new ();

        public List<Playlist> Playlists { get; set; } = new();
    }
}
