using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Releases.Commands
{
    public record SendReleaseForVerificationCommand(int ReleaseId) : ICommand<Unit>;

    public class SendReleaseForVerificationHandler : ICommandHandler<SendReleaseForVerificationCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<SendReleaseForVerificationHandler> _logger;

        public SendReleaseForVerificationHandler(IAudioStreamingContext context, ILogger<SendReleaseForVerificationHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(SendReleaseForVerificationCommand request, CancellationToken cancellationToken)
        {
            var release = await _context.Release
                .Include(r => r.Tracks)
                .SingleOrDefaultAsync(r => r.Id == request.ReleaseId);

            if (release == null)
            {
                var errorMessage = $"Release with id {request.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            if (release.Tracks.Count == 0)
            {
                return ApiResult<Unit>.CreateFailedResult("Release does not contain any tracks. Add and try again.");
            }

            if (release.Stage != ReleaseStage.Editing || release.Stage != ReleaseStage.Сorrection)
            {
                return ApiResult<Unit>.CreateFailedResult($"You cannot submit release for verification. Release is not at editing stage.");
            }

            release.Stage = ReleaseStage.Verification;
            
            await _context.SaveChangesAsync();

            return ApiResult<Unit>.CreateSuccessfulResult();

        }
    }
}
