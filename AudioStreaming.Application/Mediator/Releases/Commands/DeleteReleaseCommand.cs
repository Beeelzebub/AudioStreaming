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
    public record DeleteReleaseCommand(int ReleaseId) : ICommand<Unit>;

    public class DeleteReleaseHandler : ICommandHandler<DeleteReleaseCommand, Unit>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<DeleteReleaseHandler> _logger;

        public DeleteReleaseHandler(IAudioStreamingContext context, ILogger<DeleteReleaseHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IApiResult<Unit>> Handle(DeleteReleaseCommand request, CancellationToken cancellationToken)
        {
            var release = await _context.Release.SingleOrDefaultAsync(r => r.Id == request.ReleaseId);

            if (release == null)
            {
                var errorMessage = $"Release with id {request.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<Unit>.CreateFailedResult(errorMessage);
            }

            release.Stage = ReleaseStage.Deleted;
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult<Unit>.CreateSuccessfulResult();
        }
    }
}
