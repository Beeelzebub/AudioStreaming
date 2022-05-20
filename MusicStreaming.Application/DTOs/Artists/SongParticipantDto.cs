using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.DTOs.Artists
{
    public class SongParticipantDto
    {
        public int ArtistId { get; set; }

        public ParticipantRole Role { get; set; }

        public byte Order { get; set; }
    }
}
