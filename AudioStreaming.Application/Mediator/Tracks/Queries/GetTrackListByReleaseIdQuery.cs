using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.DTOs.Tracks;
using AudioStreaming.Application.Extensions;
using AudioStreaming.Application.Mediator.Common.Queries;
using AudioStreaming.Domain.Enums;

namespace AudioStreaming.Application.Mediator.Tracks.Queries
{
    public record GetTrackListByReleaseIdQuery(RequestParameters Parameters, int ReleaseId, bool IsOwnerRequest = false) : IQuery<PagedList<TrackDto>>;

    public class GetTrackListByReleaseIdHandler : IQueryHandler<GetTrackListByReleaseIdQuery, PagedList<TrackDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetTrackListByReleaseIdHandler(IAudioStreamingContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IApiResult<PagedList<TrackDto>>> Handle(GetTrackListByReleaseIdQuery request, CancellationToken cancellationToken)
        {
            var isReleaseExisting = request.IsOwnerRequest
                ? await _context.Release.AnyAsync(r => r.Id == request.ReleaseId, cancellationToken)
                : await _context.Release.AnyAsync(r => r.Id == request.ReleaseId && r.Stage == ReleaseStage.Published, cancellationToken);

            if (!isReleaseExisting)
            {
                var errorMessage = $"Release with id {request.ReleaseId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<PagedList<TrackDto>>.CreateFailedResult(errorMessage);
            }

            var tracks = await _context.Release
                .Where(r => r.Id == request.ReleaseId)
                .Include(r => r.Tracks)
                .Select(r => r.Tracks)
                .Include(sl => sl.SelectMany(s => s.Participants))
                    .ThenInclude(pl => pl.Artist)
                .ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<TrackDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<TrackDto>>(tracks));
        }
    }
}
