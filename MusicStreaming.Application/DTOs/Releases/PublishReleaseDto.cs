
namespace MusicStreaming.Application.DTOs.Releases
{
    public class PublishReleaseDto
    {
        public int ReleaseId { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        public bool IsScheduled { get; set; }
    }
}
