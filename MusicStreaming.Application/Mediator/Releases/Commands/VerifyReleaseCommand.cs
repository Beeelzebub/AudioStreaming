using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Abstractions.DbContexts;
using MusicStreaming.Application.Abstractions.Responses;
using MusicStreaming.Application.DTOs.Releases;
using MusicStreaming.Application.DTOs.Responses;
using MusicStreaming.Application.Mediator.Common.Commands;
using MusicStreaming.Domain.Entities;
using MusicStreaming.Domain.Enums;

namespace MusicStreaming.Application.Mediator.Releases.Commands
{
    public record VerifyReleaseCommand(VerifyReleaseDto Payload) : ICommand<Unit>;

    public class VerifyReleaseHandler : ICommandHandler<VerifyReleaseCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ILogger _logger;

        public VerifyReleaseHandler(IMusicStreamingContext context, ILogger logger)
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
