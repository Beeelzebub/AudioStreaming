using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Playlists.Commands;
using AudioStreaming.Application.Mediator.Playlists.Queries;
using AudioStreaming.Common.Extensions;
using AudioStreaming.Domain.Enums;
using AudioStreaming.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreaming.WebApi.Controllers
{
    public class PlaylistController : AudioStreamingController
    {
        public PlaylistController(IMediator mediator) : base(mediator) { }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IApiResult<PagedList<PlaylistDto>>> GetPlaylists([FromQuery] RequestParameters parameters)
        {
            var result = await _mediator.Send(new GetPlaylistListQuery(parameters));

            return result;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IApiResult<PagedList<PlaylistDto>>> GetFavoritePlaylists([FromQuery] RequestParameters parameters)
        {
            var result = await _mediator.Send(new GetFavoritePlaylistListQuery(parameters, User.GetUserId()));

            return result;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IApiResult<int>> CreatePlaylist([FromBody] CreatePlaylistDto payload)
        {
            var result = await _mediator.Send(new CreatePlaylistCommand(payload, User.GetUserId()));

            return result;
        }

        [HttpPatch("[action]")]
        [Authorize]
        [CheckPlaylistPermissionFilter(PermissionType = PermissionType.Edit)]
        public async Task<IApiResult> EditPlaylist([FromBody] EditPlaylistDto payload)
        {
            var result = await _mediator.Send(new EditPlaylistCommand(payload));

            return result;
        }
    }
}
