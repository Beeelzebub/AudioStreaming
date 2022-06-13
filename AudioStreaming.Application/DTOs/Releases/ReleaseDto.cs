using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Releases
{
    public class ReleaseDto
    {
        public int ReleaseId { get; set; }

        public DateTimeOffset Date { get; set; }

        public ReleaseType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ReleaseCoverUri { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ReleaseStage ReleaseStage { get; set; }

        public IEnumerable<ReleaseParticipantDto> Participants { get; set; } = new List<ReleaseParticipantDto>();
    }

    public class ReleaseParticipantDto
    {
        public string Id { get; set; }

        public string Pseudonym { get; set; } = string.Empty;
    }
}
