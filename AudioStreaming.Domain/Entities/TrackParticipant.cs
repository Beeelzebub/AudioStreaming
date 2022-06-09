using AudioStreaming.Domain.Abstractions;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Domain.Entities
{
    public class TrackParticipant : IEntity
    {
        public byte Order { get; set; }

        public int ArtistId { get; set; }

        public int TrackId { get; set; }

        public ParticipantRole Role { get; set; }

        public Artist Artist { get; set; } = default!;

        public Track Track { get; set; } = default!;
    }
}
