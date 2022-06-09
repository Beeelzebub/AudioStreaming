using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public record GetTrackListByGenresQuery(RequestParameters Parameters, IEnumerable<string> Genres) : IQuery<PagedList<TrackDto>>;

    public class GetTrackListByGenresHandler : IQueryHandler<GetTrackListByGenresQuery, PagedList<TrackDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;

        public GetTrackListByGenresHandler(IAudioStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<PagedList<TrackDto>>> Handle(GetTrackListByGenresQuery request, CancellationToken cancellationToken)
        {
            var tracks = await _context.Track
                .Where(s => s.Release.Stage == ReleaseStage.Published)
                .Where(s => s.Genres.Select(g => g.Name).Any(g => request.Genres.Contains(g)))
                .Include(s => s.Release)
                .Include(s => s.Participants)
                    .ThenInclude(pl => pl.Artist)
                .OrderBy(s => s.ListeningHistory.Count())
                .ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<TrackDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<TrackDto>>(tracks));
        }
    }
}
