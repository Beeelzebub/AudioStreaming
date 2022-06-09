using AudioStreaming.Domain.Abstractions;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Domain.Entities
{
    public class Release : IEntity
    {
        public int Id { get; set; }

        public DateTimeOffset? Date { get; set; }

        public ReleaseType Type { get; set; }

        public ReleaseStage Stage { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; } = default!;

        public string? ReleaseCoverUri { get; set; } = default!;

        public List<Track> Tracks { get; set; } = new();

        public List<Artist> Artists { get; set; } = new();

        public List<ReleaseParticipant> Participants { get; set; } = default!;

        public List<User> UsersWhoAddedToFavorite { get; set; } = default!;

        public List<ReleaseVerificationHistory> VerificationHistory { get; set; } = new();
    }
}
