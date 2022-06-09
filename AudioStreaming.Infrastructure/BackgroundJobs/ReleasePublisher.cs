using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Infrastructure.BackgroundJobs
{
    public class ReleasePublisher
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;

        public ReleasePublisher(IAudioStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void PublishRelease(int releaseId, DateTimeOffset publishDate)
        {
            var release = _context.Release.SingleOrDefault(r => r.Id == releaseId);

            if (release == null)
            {
                _logger.LogError($"Release publishing failed: Release with id {releaseId} not found.");
                return;
            }

            if (release.Stage != ReleaseStage.Approved)
            {
                _logger.LogError($"Release publishing failed: Release is at {release.Stage} stage.");
                return;
            }

            release.Date = publishDate;
            release.Stage = ReleaseStage.Released;

            _context.SaveChanges();
        }
    }
}
