namespace AudioStreaming.Application.DTOs.Releases
{
    public class PublishReleaseDto : ReleaseBaseDto
    {
        public DateTimeOffset PublishDate { get; set; }

        public bool IsScheduled { get; set; }
    }
}
