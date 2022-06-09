using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Tracks.Commands
{
    public record DeleteTrackFromFavoriteCommand(int UserId, int TrackId) : ICommand<Unit>;

    public class DeleteTrackFromFavoriteHandler : ICommandHandler<DeleteTrackFromFavoriteCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;

        public DeleteTrackFromFavoriteHandler(IAudioStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(DeleteTrackFromFavoriteCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.User
                .Include(u => u.FavoriteTracks)
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                var errorMessage = $"User with id {request.UserId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            var trackToRemove = user.FavoriteTracks.SingleOrDefault(t => t.Id == request.TrackId);

            if (trackToRemove != null)
            {
                user.FavoriteTracks.Remove(trackToRemove);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
