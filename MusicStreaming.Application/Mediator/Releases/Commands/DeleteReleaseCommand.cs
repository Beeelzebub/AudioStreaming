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
    public record DeleteReleaseCommand(int ReleaseId) : ICommand<Unit>;

    public class DeleteReleaseHandler : ICommandHandler<DeleteReleaseCommand, Unit>
    {
        private readonly IMusicStreamingContext _context;
        private readonly ILogger _logger;

        public DeleteReleaseHandler(IMusicStreamingContext context, ILogger logger)
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
