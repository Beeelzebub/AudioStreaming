using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Responses;
using MusicStreaming.Application.Abstractions.Services.Releases;
using MusicStreaming.Application.DTOs.Releases;
using MusicStreaming.Application.DTOs.Responses;
using MusicStreaming.Application.Mediator.Common.Commands;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.Mediator.Releases.Commands
{
    public record PublishReleaseCommand(PublishReleaseDto Payload) : ICommand<Unit>;

    public class PublishReleaseHandler : ICommandHandler<PublishReleaseCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly IReleaseService _releaseService;
        private readonly ILogger _logger;

        public PublishReleaseHandler(IMusicStreamingContext context, IReleaseService releaseService, ILogger logger)
        {
            _releaseService = releaseService;
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(PublishReleaseCommand request, CancellationToken cancellationToken)
        {
            var release = await _context.Release
                .Include(r => r.Artists)
                .SingleOrDefaultAsync(r => r.Id == request.Payload.ReleaseId);

            if (release == null)
            {
                var errorMessage = $"Release with id {request.Payload.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            if (release.Stage != ReleaseStage.Approved)
            {
                return ApiResult<Unit>.CreateFailedResult($"Release not approved. You cannot publish it.");
            }

            if (request.Payload.IsScheduled)
            {
                _releaseService.PublishScheduledRelease(release.Id, request.Payload.PublishDate);
            }
            else
            {
                await _releaseService.PublishReleaseAsync(release, cancellationToken);
            }

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
