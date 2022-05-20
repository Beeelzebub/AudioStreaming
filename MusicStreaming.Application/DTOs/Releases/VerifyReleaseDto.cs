using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.DTOs.Releases
{
    public class VerifyReleaseDto
    {
        public int ReleaseId { get; set; }

        public ReleaseStage NewStage { get; set; }

        public string Comment { get; set; }
    }
}
