using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.DTOs.Tracks;
using AudioStreaming.Application.Mediator.Common.Queries;
using AudioStreaming.Application.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Tracks.Queries
{
    public record GetTrackListByPlaylistIdQuery(RequestParameters Parameters, int PlaylistId) : IQuery<PagedList<TrackDto>>;

    public class GetTrackListByPlaylistIdHandler : IQueryHandler<GetTrackListByPlaylistIdQuery, PagedList<TrackDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTrackListByPlaylistIdHandler> _logger;

        public GetTrackListByPlaylistIdHandler(IAudioStreamingContext context, IMapper mapper, ILogger<GetTrackListByPlaylistIdHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IApiResult<PagedList<TrackDto>>> Handle(GetTrackListByPlaylistIdQuery request, CancellationToken cancellationToken)
        {
            var isPlaylistExisting = await _context.Playlist
                .AnyAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (!isPlaylistExisting)
            {
                var errorMessage = $"Playlist with id {request.PlaylistId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<PagedList<TrackDto>>.CreateFailedResult(errorMessage);
            }

            var tracks = await _context.Playlist
                .AsNoTracking()
                .Where(p => p.Id == request.PlaylistId)
                .Include(p => p.Tracks)
                .Select(p => p.Tracks)
                .Include(sl => sl.Select(s => s.Release))
                .Include(sl => sl.SelectMany(s => s.Participants))
                    .ThenInclude(pl => pl.Artist)
                .ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<TrackDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<TrackDto>>(tracks));
        }
    }
}
