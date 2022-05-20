
using MusicStreaming.Domain.Entities;

namespace MusicStreaming.Application.Abstractions.Services.Releases
{
    public interface IReleaseService
    {
        void PublishScheduledRelease(int releaseId, DateTimeOffset publishDate);

        Task PublishReleaseAsync(Release release, CancellationToken cancellationToken = default);
    }
}
