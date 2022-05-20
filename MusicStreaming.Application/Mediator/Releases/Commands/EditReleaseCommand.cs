using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Response;
using MusicStreaming.Application.Abstractions.Services.BlobStorage;
using MusicStreaming.Application.DTOs.Releases;
using MusicStreaming.Application.DTOs.Response;
using MusicStreaming.Application.Mediator.Common.Commands;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.Mediator.Releases.Commands
{
    public record EditReleaseCommand(EditReleaseDto Release) : ICommand<Unit>;

    public class EditReleaseHandler : ICommandHandler<EditReleaseCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ICoverBlobService _coverBlobService;
        private readonly ILogger _logger;

        public EditReleaseHandler(IMusicStreamingContext context, ILogger logger, ICoverBlobService coverBlobService)
        {
            _context = context;
            _logger = logger;
            _coverBlobService = coverBlobService;
        }

        public async Task<IApiResult<Unit>> Handle(EditReleaseCommand request, CancellationToken cancellationToken)
        {
            var release = await _context.Release
                .Include(r => r.Songs)
                .Include(r => r.Artists)
                .SingleOrDefaultAsync(r => r.Id == request.Release.ReleaseId);

            if (release == null)
            {
                var errorMessage = $"Release with id {request.Release.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            if (release.Stage != ReleaseStage.Editing)
            {
                return ApiResult<Unit>.CreateFailedResult("Release can only be edited at the editing stage.");
            }

            if (release.Type == ReleaseType.Album && release.Songs.Count > 1 && request.Release.Type == ReleaseType.Single)
            {
                return ApiResult<Unit>.CreateFailedResult("The specified release type is album and you have already added several tracks earlier. " +
                    "Leave one track and try again.");
            }

            release.Title = request.Release.Title;
            release.Description = request.Release.Description;
            release.Type = request.Release.Type;

            if (request.Release.CoverFile != null)
            {
                await UpdateReleaseCover(release.Id, request.Release.CoverFile);
            }

            return ApiResult<Unit>.CreateSuccessfulResult();
        }

        private async Task<string> UpdateReleaseCover(int releaseId, IFormFile cover)
        {
            await using var stream = new MemoryStream();
            await cover.CopyToAsync(stream);

            return await _coverBlobService.UpdateReleaseCover(releaseId, stream);
        }
    }
}
