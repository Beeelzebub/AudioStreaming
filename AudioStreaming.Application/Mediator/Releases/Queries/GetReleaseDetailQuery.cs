using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Common.Queries;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Releases.Queries
{
    public record GetReleaseDetailQuery(int ReleaseId, bool RequestWithPermissions) : IQuery<ReleaseDetailDto>;

    public class GetReleaseDetailHandler : IQueryHandler<GetReleaseDetailQuery, ReleaseDetailDto>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger<GetReleaseDetailHandler> _logger;
        private readonly IMapper _mapper;

        public GetReleaseDetailHandler(IAudioStreamingContext context, ILogger<GetReleaseDetailHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IApiResult<ReleaseDetailDto>> Handle(GetReleaseDetailQuery request, CancellationToken cancellationToken)
        {
            var release = await _context.Release
                .Include(r => r.Tracks)
                    .ThenInclude(sl => sl.SelectMany(s => s.Participants))
                    .ThenInclude(pl => pl.Artist)
                .Include(r => r.Artists)
                .SingleOrDefaultAsync(r => r.Id == request.ReleaseId);

            if (release == null)
            {
                var errorMessage = $"Release with id {request.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<ReleaseDetailDto>.CreateFailedResult(errorMessage);
            }

            if (!request.RequestWithPermissions && release.Stage != ReleaseStage.Published)
            {
                return ApiResult<ReleaseDetailDto>.CreateFailedResult("You have no permissions.");
            }

            return ApiResult<ReleaseDetailDto>.CreateSuccessfulResult(_mapper.Map<ReleaseDetailDto>(release));
        }
    }
}
