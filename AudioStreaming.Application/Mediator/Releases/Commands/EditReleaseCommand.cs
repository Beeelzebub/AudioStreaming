using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.BlobStorage;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Releases.Commands
{
    public record EditReleaseCommand(EditReleaseDto Release) : ICommand<Unit>;

    public class EditReleaseHandler : ICommandHandler<EditReleaseCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ICoverBlobService _coverBlobService;
        private readonly ILogger _logger;

        public EditReleaseHandler(IAudioStreamingContext context, ILogger logger, ICoverBlobService coverBlobService)
        {
            _context = context;
            _logger = logger;
            _coverBlobService = coverBlobService;
        }

        public async Task<IApiResult<Unit>> Handle(EditReleaseCommand request, CancellationToken cancellationToken)
        {
            var release = await _context.Release
                .Include(r => r.Tracks)
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

            if (request.Release.Type != null && request.Release.Type == ReleaseType.Single && release.Type == ReleaseType.Album && release.Tracks.Count > 1)
            {
                return ApiResult<Unit>.CreateFailedResult("The specified release type is album and you have already added several tracks earlier. " +
                    "Leave one track and try again.");
            }

            release.Title = request.Release.Title ?? release.Title;
            release.Description = request.Release.Description ?? release.Description;
            release.Type = request.Release.Type ?? release.Type;

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

            return await _coverBlobService.UploadReleaseCoverAsync(releaseId, stream);
        }
    }
}
