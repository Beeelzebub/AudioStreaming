using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Responses;
using MusicStreaming.Application.DTOs.Responses;
using MusicStreaming.Application.Mediator.Common.Commands;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.Mediator.Releases.Commands
{
    public record SendReleaseForVerificationCommand(int ReleaseId) : ICommand<Unit>;

    public class SendReleaseForVerificationHandler : ICommandHandler<SendReleaseForVerificationCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ILogger _logger;

        public SendReleaseForVerificationHandler(IMusicStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(SendReleaseForVerificationCommand request, CancellationToken cancellationToken)
        {
            var release = await _context.Release
                .Include(r => r.Songs)
                .SingleOrDefaultAsync(r => r.Id == request.ReleaseId);

            if (release == null)
            {
                var errorMessage = $"Release with id {request.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            if (release.Songs.Count == 0)
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
