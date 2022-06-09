using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Extensions;
using AudioStreaming.Application.Mediator.Common.Queries;
using AutoMapper;

namespace AudioStreaming.Application.Mediator.Playlists.Queries
{
    public record GetPlaylistListQuery(RequestParameters Parameters) : IQuery<PagedList<PlaylistDto>>;

    public class GetPlaylistListHandler : IQueryHandler<GetPlaylistListQuery, PagedList<PlaylistDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly IMapper _mapper;

        public GetPlaylistListHandler(IAudioStreamingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IApiResult<PagedList<PlaylistDto>>> Handle(GetPlaylistListQuery request, CancellationToken cancellationToken)
        {
            var playlistsQuery = _context.Playlist.Where(p => !p.IsPrivate);

            if (!string.IsNullOrEmpty(request.Parameters.SearchString))
            {
                playlistsQuery = playlistsQuery.Where(p => p.Title.Contains(request.Parameters.SearchString));
            }

            var playlists = await playlistsQuery.ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<PlaylistDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<PlaylistDto>>(playlists));
        }
    }
}
