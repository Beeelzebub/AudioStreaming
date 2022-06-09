using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Tracks.Commands
{
    public record AddTracksToPlaylistCommand(IEnumerable<int> TrackIds, int PlaylistId) : ICommand<Unit>;

    public class AddTracksToPlaylistHandler : ICommandHandler<AddTracksToPlaylistCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;

        public AddTracksToPlaylistHandler(IAudioStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(AddTracksToPlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlist.SingleOrDefaultAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (playlist == null)
            {
                var errorMessage = $"Playlist with id {request.PlaylistId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            var Tracks = await _context.Track
                .Where(s => s.Release.Stage == ReleaseStage.Published)
                .Where(s => request.TrackIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            var existingIds = Tracks.Select(s => s.Id);

            CheckTracks(existingIds, request.TrackIds, out var nonexistentIdsMessage);

            playlist.Tracks.AddRange(Tracks);

            await _context.SaveChangesAsync(cancellationToken);

            var result = ApiResult<Unit>.CreateSuccessfulResult();

            result.Errors = string.IsNullOrEmpty(nonexistentIdsMessage) ? new List<string> { nonexistentIdsMessage } : null;

            return result;
        }

        private void CheckTracks(IEnumerable<int> existingIds, IEnumerable<int> requestdIds, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (existingIds.Count() != requestdIds.Count())
            {
                var nonexistentIds = requestdIds.Except(existingIds);
                var ending = nonexistentIds.Count() > 1 ? "s" : "";
                errorMessage = $"Track{ending} with id{ending} {String.Join(',', nonexistentIds)} not found.";

                _logger.LogError(errorMessage);
            }
        }
    }
}
