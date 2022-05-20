using Microsoft.AspNetCore.Http;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.DTOs.Releases
{
    public class EditReleaseDto
    {
        public int ReleaseId { get; set; }

        public ReleaseType Type { get; set; }

        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;

        public IFormFile? CoverFile { get; set; } = default!;
    }
}
