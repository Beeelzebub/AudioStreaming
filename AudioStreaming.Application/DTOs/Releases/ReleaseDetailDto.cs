using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Releases
{
    public class ReleaseDetailDto : ReleaseDto
    {
        public string Description { get; set; } = string.Empty;

        public ReleaseStage ReleaseStage { get; set; }
    }
}
