using AudioStreaming.Application.Abstractions.Services.Releases;
using AudioStreaming.Infrastructure.BackgroundJobs;
using Hangfire;

namespace AudioStreaming.Infrastructure.Services.Releases
{
    public class ReleaseService : IReleaseService
    {
        private readonly ReleasePublisher _releasePublisher;

        public ReleaseService(ReleasePublisher releasePublisher)
        {
            _releasePublisher = releasePublisher;
        }

        public void PublishScheduledRelease(int releaseId, DateTimeOffset publishDate)
        {
            BackgroundJob.Schedule(() => _releasePublisher.PublishRelease(releaseId, publishDate), publishDate);
        }
    }
}
