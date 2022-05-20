using MusicStreaming.Domain.Abstractions;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Domain.Entities
{
    public class SongParticipant : IEntity
    {
        public byte Order { get; set; }

        public int ArtistId { get; set; }

        public int SongId { get; set; }

        public ParticipantRole Role { get; set; }

        public Artist Artist { get; set; } = default!;

        public Song Song { get; set; } = default!;
    }
}
