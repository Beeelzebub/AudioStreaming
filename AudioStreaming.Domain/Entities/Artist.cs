using AudioStreaming.Domain.Abstractions;

namespace AudioStreaming.Domain.Entities
{
    public class Artist : IEntity
    {
        public int Id { get; set; }

        public bool IsConfirmed { get; set; }   

        public string Pseudonym { get; set; } = default!;

        public string Email { get; set; } = default;

        public string? FullName { get; set; } = default;

        public string? Country { get; set; } = default;

        public string? Description { get; set; } = default;

        public string? ProfileImageUri { get; set; } = default;

        public List<TrackParticipant> ParticipatingInTracks { get; set; } = default!;

        public List<ReleaseParticipant> ParticipatingInReleases { get; set; } = default!;

        public List<Release> Releases { get; set; } = default!;

        public List<Track> Tracks { get; set; } = default!;
    }
}
