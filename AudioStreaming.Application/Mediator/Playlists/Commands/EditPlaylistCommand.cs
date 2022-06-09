using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;

namespace AudioStreaming.Application.Mediator.Playlists.Commands
{
    public record EditPlaylistCommand(EditPlaylistDto Payload) : ICommand<Unit>;

    public class EditPlaylistHandler : ICommandHandler<EditPlaylistCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ICoverBlobService _coverBlobService;
        private readonly ILogger _logger;

        public EditPlaylistHandler(IAudioStreamingContext context, ILogger logger, ICoverBlobService coverBlobService)
        {
            _context = context;
            _logger = logger;
            _coverBlobService = coverBlobService;
        }

        public async Task<IApiResult<Unit>> Handle(EditPlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlist.SingleOrDefaultAsync(p => p.Id == request.Payload.PlaylistId);

            if (playlist == null)
            {
                var errorMessage = $"Playlist with id {request.Payload.PlaylistId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            playlist.IsPrivate = request.Payload.IsPrivate;
            playlist.Title = request.Payload.Title;
            playlist.Description = request.Payload.Description;

            if (request.Payload.PlaylistCoverFile != null)
            {
                playlist.PlaylistCoverUri = await UpdatePlaylistCoverAsync(playlist.Id, request.Payload.PlaylistCoverFile);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult<Unit>.CreateSuccessfulResult();
        }

        private async Task<string> UpdatePlaylistCoverAsync(int playlistId, IFormFile cover)
        {
            await using var stream = new MemoryStream();
            await cover.CopyToAsync(stream);

            return await _coverBlobService.UploadPlaylistCoverAsync(playlistId, stream);
        }
    }
}
