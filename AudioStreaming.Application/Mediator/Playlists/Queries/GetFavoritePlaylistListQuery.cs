using AudioStreaming.Application.Abstractions.DbContexts;
using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Extensions;
using AudioStreaming.Application.Mediator.Common.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AudioStreaming.Application.Mediator.Playlists.Queries
{
    public record GetFavoritePlaylistListQuery(RequestParameters Parameters, string UserId) : IQuery<PagedList<PlaylistDto>>;

    public class GetFavoritePlaylistListHandler : IQueryHandler<GetFavoritePlaylistListQuery, PagedList<PlaylistDto>>
    {
        private readonly IAudioStreamingContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetFavoritePlaylistListHandler(IAudioStreamingContext context, IMapper mapper, ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IApiResult<PagedList<PlaylistDto>>> Handle(GetFavoritePlaylistListQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Id == request.UserId);

            if (user == null)
            {
                var errorMessage = $"User with id {request.UserId} not found.";

                _logger.LogError(errorMessage);

                return ApiResult<PagedList<PlaylistDto>>.CreateFailedResult(errorMessage);
            }

            var playlistsQuery = _context.Playlist
                .Where(p => p.UsersWhoAddedToFavorite.Contains(user));

            if (!string.IsNullOrEmpty(request.Parameters.SearchString))
            {
                playlistsQuery = playlistsQuery.Where(p => p.Title.Contains(request.Parameters.SearchString));
            }

            var playlists = playlistsQuery.ToPagedListAsync(request.Parameters.Page, request.Parameters.PageSize, cancellationToken);

            return ApiResult<PagedList<PlaylistDto>>.CreateSuccessfulResult(_mapper.Map<PagedList<PlaylistDto>>(playlists));
        }
    }
}
