using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Tracks.Commands
{
    public record DeleteTrackFromPlaylistCommand(int PlaylistId, int TrackId) : ICommand<Unit>;

    public class DeleteTrackFromPlaylistHandler : ICommandHandler<DeleteTrackFromPlaylistCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<DeleteTrackFromPlaylistHandler> _logger;

        public DeleteTrackFromPlaylistHandler(IAudioStreamingContext context, ILogger<DeleteTrackFromPlaylistHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(DeleteTrackFromPlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlist
                .Include(p => p.Tracks)
                .SingleOrDefaultAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (playlist == null)
            {
                var errorMessage = $"Playlist with id {request.PlaylistId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            var trackToRemove = playlist.Tracks.SingleOrDefault(t => t.Id == request.TrackId);

            if (trackToRemove != null)
            {
                playlist.Tracks.Remove(trackToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}

