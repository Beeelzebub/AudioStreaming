using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Artists
{
    public class TrackParticipantDto
    {
        public int ArtistId { get; set; }

        public ParticipantRole Role { get; set; }

        public byte Order { get; set; }
    }
}
