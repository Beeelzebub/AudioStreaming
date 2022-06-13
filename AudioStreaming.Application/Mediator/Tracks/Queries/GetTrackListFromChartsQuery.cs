using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.DTOs.Tracks;
using AudioStreaming.Application.Extensions;
using AudioStreaming.Application.Mediator.Common.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AudioStreaming.Application.Mediator.Tracks.Queries
{
    public record GetTrackListFromChartsQuery(RequestParameters Parameters) : IQuery<PagedList<TrackDto>>;

    public class GetTrackListFromChartsHandler : IQueryHandler<GetTrackListFromChartsQuery, PagedList<TrackDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;

        public GetTrackListFromChartsHandler(IAudioStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<PagedList<TrackDto>>> Handle(GetTrackListFromChartsQuery request, CancellationToken cancellationToken)
        {
            var tracks = await _context.Track
                .AsNoTracking()
                .Where(c => c.PositionInChart != null)
                .Include(t => t.Release)
                .Include(t => t.Genres)
                .Include(t => t.Participants)
                    .ThenInclude(pl => pl.Artist)
                .OrderByDescending(t => t.PositionInChart)
                .ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<TrackDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<TrackDto>>(tracks));
        }
    }
}
