using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Playlists.Commands
{
    public record CreatePlaylistCommand(CreatePlaylistDto Playlist, string OwnerId) : ICommand<int>;

    public class CreatePlaylistHandler : ICommandHandler<CreatePlaylistCommand, int>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ICoverBlobService _coverBlobService;
        private readonly ILogger _logger;

        public CreatePlaylistHandler(IAudioStreamingContext context, ILogger logger, ICoverBlobService coverBlobService)
        {
            _context = context;
            _logger = logger;
            _coverBlobService = coverBlobService;
        }

        public async Task<IApiResult<int>> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Id == request.OwnerId);

            if (user == null)
            {
                var errorMessage = $"User with id {request.OwnerId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<int>.CreateFailedResult(errorMessage);
            }

            var Tracks = await _context.Track
                .Where(s => s.Release.Stage == ReleaseStage.Published)
                .Where(s => request.Playlist.TrackIds.Contains(s.Id))
                .ToListAsync();

            var existingIds = Tracks.Select(s => s.Id);

            CheckTracks(existingIds, request.Playlist.TrackIds, out var nonexistentIdsMessage);

            var ownerPermission = new PlaylistPermission
            {
                User = user,
                Type = PermissionType.Edit,
                Name = $"Playlist owner permission"
            };

            var playlistToAdd = new Playlist()
            {
                Title = request.Playlist.Title,
                Description = request.Playlist.Description,
                Tracks = Tracks,
                IsPrivate = request.Playlist.IsPrivate,
                OwnerId = request.OwnerId,
                Permissions = new List<PlaylistPermission> { ownerPermission },
                UsersWhoAddedToFavorite = new List<User> { user }
            };

            await _context.SaveChangesAsync(cancellationToken);

            if (request.Playlist.PlaylistCoverFile != null)
            {
                playlistToAdd.PlaylistCoverUri = await UploadPlaylistCover(playlistToAdd.Id, request.Playlist.PlaylistCoverFile);

                await _context.SaveChangesAsync(cancellationToken);
            }

            var result = ApiResult<int>.CreateSuccessfulResult();

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

        private async Task<string> UploadPlaylistCover(int playlistId, IFormFile cover)
        {
            await using var stream = new MemoryStream();
            await cover.CopyToAsync(stream);

            return await _coverBlobService.UploadPlaylistCoverAsync(playlistId, stream);
        }
    }
}
