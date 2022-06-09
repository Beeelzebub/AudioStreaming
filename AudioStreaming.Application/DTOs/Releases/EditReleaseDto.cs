using Microsoft.AspNetCore.Http;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.DTOs.Releases
{
    public class EditReleaseDto : ReleaseBaseDto
    {

        public ReleaseType? Type { get; set; } = default;

        public string? Title { get; set; } = default;

        public string? Description { get; set; } = default;

        public IFormFile? CoverFile { get; set; } = default;
    }
}
