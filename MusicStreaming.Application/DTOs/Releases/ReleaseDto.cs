using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.DTOs.Releases
{
    public class ReleaseDto
    {
        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public ReleaseType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ReleaseCoverUri { get; set; }

        public IEnumerable<ReleaseParticipantDto> Participants { get; set; } = new List<ReleaseParticipantDto>();
    }

    public class ReleaseParticipantDto
    {
        public int Id { get; set; }

        public string Pseudonym { get; set; }
    }
}
