using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Commands;
using AudioStreaming.Domain.Entities;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Releases.Commands
{
    public record VerifyReleaseCommand(VerifyReleaseDto Payload) : ICommand<Unit>;

    public class VerifyReleaseHandler : ICommandHandler<VerifyReleaseCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;

        public VerifyReleaseHandler(IAudioStreamingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(VerifyReleaseCommand request, CancellationToken cancellationToken)
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

            if (release.Stage != ReleaseStage.Verification)
            {
                return ApiResult<Unit>.CreateFailedResult($"Release is not at verification stage. You cannot verify it.");
            }

            var verificationResult = new ReleaseVerificationHistory
            {
                Comment = request.Payload.Comment,
                Date = DateTimeOffset.Now,
                NewStage = request.Payload.NewStage
            };
            
            release.VerificationHistory.Add(verificationResult);
            release.Stage = request.Payload.NewStage;

            await _context.SaveChangesAsync();

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}   
