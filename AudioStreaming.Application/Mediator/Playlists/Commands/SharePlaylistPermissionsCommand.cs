using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Playlists.Commands
{
    public record SharePlaylistPermissionsCommand(ICollection<SharePlaylistPermissionsDto> Payload, int PlaylistId) : ICommand<Unit>;

    public class SharePlaylistPermissionsHandler : ICommandHandler<SharePlaylistPermissionsCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<SharePlaylistPermissionsHandler> _logger;

        public SharePlaylistPermissionsHandler(IAudioStreamingContext context, ILogger<SharePlaylistPermissionsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IApiResult<Unit>> Handle(SharePlaylistPermissionsCommand request, CancellationToken cancellationToken)
        {
            var userIds = request.Payload.Select(x => x.UserId);

            var users = await _context.User
                .Include(u => u.Permissions)
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var playlistExists = await _context.Playlist.AnyAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (!playlistExists)
            {
                var errorMessage = $"Playlist with id {request.PlaylistId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            var errors = new List<string>();

            foreach (var permission in request.Payload)
            {
                var user = users.FirstOrDefault(u => u.Id == permission.UserId);

                if (user == null)
                {
                    errors.Add($"User with id {permission.UserId} not found.");
                    continue;
                }

                if (user.Permissions.Any(p => p.PlaylistId == request.PlaylistId && p.Type == permission.PermissionType))
                {
                    errors.Add($"User with id {permission.UserId} already has {permission.PermissionType} permission.");
                    continue;
                }

                user.Permissions.Add(new PlaylistPermission() { PlaylistId = request.PlaylistId, Type = permission.PermissionType });
            }

            var changedRecordsCount = await _context.SaveChangesAsync(cancellationToken);

            return new ApiResult<Unit>
            {
                IsSuccess = changedRecordsCount > 0,
                Errors = errors.Count > 0 ? errors : null
            };
        }
    }
}
