using Microsoft.AspNetCore.Http;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.DTOs.Releases
{
    public class CreateReleaseDto
    {
        public DateTimeOffset Date { get; set; }

        public ReleaseType Type { get; set; }

        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;

        public IFormFile? ReleaseCoverUri { get; set; } = default;

        public IEnumerable<int> ParticipantsIds { get; set; } = default!;
    }
}
