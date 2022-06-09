namespace AudioStreaming.Application.Abstractions.Services.Releases
{
    public interface IReleaseService
    {
        void PublishScheduledRelease(int releaseId, DateTimeOffset publishDate);
    }
}
