using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.Abstractions.Services.Releases;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Releases.Commands
{
    public record PublishReleaseCommand(PublishReleaseDto Payload) : ICommand<Unit>;

    public class PublishReleaseHandler : ICommandHandler<PublishReleaseCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IReleaseService _releaseService;
        private readonly ILogger<PublishReleaseHandler> _logger;

        public PublishReleaseHandler(IAudioStreamingContext context, IReleaseService releaseService, ILogger<PublishReleaseHandler> logger)
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
                release.Date = DateTimeOffset.Now;
                release.Stage = ReleaseStage.Published;

                await _context.SaveChangesAsync();
            }

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
