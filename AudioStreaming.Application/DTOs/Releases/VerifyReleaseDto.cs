using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Releases
{
    public class VerifyReleaseDto : ReleaseBaseDto
    {
        public ReleaseStage NewStage { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}
