using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Tracks.Commands
{
    public record DeleteTrackFromReleaseCommand(int ReleaseId, int TrackId) : ICommand<Unit>;

    public class DeleteTrackFromReleaseHandler : ICommandHandler<DeleteTrackFromReleaseCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ITrackBlobService _trackBlobService;
        private readonly ILogger<DeleteTrackFromReleaseHandler> _logger;

        public DeleteTrackFromReleaseHandler(IAudioStreamingContext context, ILogger<DeleteTrackFromReleaseHandler> logger, ITrackBlobService trackBlobService)
        {
            _context = context;
            _logger = logger;
            _trackBlobService = trackBlobService;
        }

        public async Task<IApiResult<Unit>> Handle(DeleteTrackFromReleaseCommand request, CancellationToken cancellationToken)
        {
            var track = await _context.Release
                .Where(r => r.Id == request.ReleaseId)
                .SelectMany(r => r.Tracks)
                .SingleOrDefaultAsync(t => t.Id == request.TrackId);

            if (track == null)
            {
                var errorMessage = $"Track with id {request.TrackId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            await _trackBlobService.DeleteIfExistsAsync(track.PathInStorage);

            _context.Track.Remove(track);
            await _context.SaveChangesAsync(cancellationToken);
            
            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}

