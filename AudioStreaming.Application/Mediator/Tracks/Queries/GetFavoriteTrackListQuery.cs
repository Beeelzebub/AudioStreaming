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

namespace AudioStreaming.Application.Mediator.Tracks.Queries
{
    public record GetFavoriteTrackListQuery(RequestParameters Parameters, string UserId) : IQuery<PagedList<TrackDto>>;

    public class GetFavoriteTrackListHandler : IQueryHandler<GetFavoriteTrackListQuery, PagedList<TrackDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFavoriteTrackListHandler> _logger;

        public GetFavoriteTrackListHandler(IAudioStreamingContext context, IMapper mapper, ILogger<GetFavoriteTrackListHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IApiResult<PagedList<TrackDto>>> Handle(GetFavoriteTrackListQuery request, CancellationToken cancellationToken)
        {
            var isUserExisting = await _context.User.AnyAsync(u => u.Id == request.UserId, cancellationToken);

            if (!isUserExisting)
            {
                return ApiResult<PagedList<TrackDto>>.CreateFailedResult($"User with id {request.UserId} not found.");
            }

            var tracks = await _context.User
                .Where(u => u.Id == request.UserId)
                .Include(u => u.FavoriteTracks)
                .Select(u => u.FavoriteTracks)
                .Include(sl => sl.SelectMany(s => s.Participants))
                    .ThenInclude(pl => pl.Artist)
                .ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<TrackDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<TrackDto>>(tracks));
        }
    }
}
