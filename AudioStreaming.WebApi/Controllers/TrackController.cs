using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Playlists;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.DTOs.Tracks;
using AudioStreaming.Application.Mediator.Tracks.Commands;
using AudioStreaming.Application.Mediator.Tracks.Queries;
using AudioStreaming.Common.Extensions;
using AudioStreaming.Domain.Enums;
using AudioStreaming.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Security;

namespace AudioStreaming.WebApi.Controllers
{
    public class TrackController : AudioStreamingController
    {
        public TrackController(IMediator mediator) : base(mediator) { }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IApiResult<PagedList<TrackDto>>> GetFavoriteTracks([FromQuery] RequestParameters parameters)
        {
            var result = await _mediator.Send(new GetFavoriteTrackListQuery(parameters, User.GetUserId()));

            return result;
        }

        [HttpGet("[action]")]
        public async Task<IApiResult<PagedList<TrackDto>>> GetTracksByGenres([FromQuery] RequestParameters parameters, [FromQuery] ICollection<string> genres)
        {
            var result = await _mediator.Send(new GetTrackListByGenresQuery(parameters, genres));

            return result;
        }

        [HttpGet("[action]")]
        [Authorize]
        [CheckPlaylistPermissionFilter(PermissionType = PermissionType.Read)]
        public async Task<IApiResult<PagedList<TrackDto>>> GetTracksByPlaylist([FromQuery] RequestParameters parameters, [FromQuery] PlaylistBaseDto payload)
        {
            var result = await _mediator.Send(new GetTrackListByPlaylistIdQuery(parameters, payload.PlaylistId));

            return result;
        }

        [HttpGet("[action]")]
        public async Task<IApiResult<PagedList<TrackDto>>> GetTracksByRelease([FromQuery] RequestParameters parameters, [FromQuery] ReleaseBaseDto payload)
        {
            var result = await _mediator.Send(new GetTrackListByReleaseIdQuery(parameters, payload.ReleaseId, false));

            return result;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = AudioStreamingRoles.Artist)]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult<PagedList<TrackDto>>> GetOwnTracksByRelease([FromQuery] RequestParameters parameters, [FromQuery] ReleaseBaseDto payload)
        {
            var result = await _mediator.Send(new GetTrackListByReleaseIdQuery(parameters, payload.ReleaseId, true));

            return result;
        }

        [HttpGet("[action]")]
        public async Task<IApiResult<PagedList<TrackDto>>> GetTracksFromCharts([FromQuery] RequestParameters parameters)
        {
            var result = await _mediator.Send(new GetTrackListFromChartsQuery(parameters));

            return result;
        }

        [HttpPost("[action]")]
        [Authorize]
        [CheckPlaylistPermissionFilter(PermissionType = PermissionType.Edit)]
        public async Task<IApiResult> AddTracksToPlaylist([FromBody] AddTracksToPlaylistDto payload)
        {
            var result = await _mediator.Send(new AddTracksToPlaylistCommand(payload.TrackIds, payload.PlaylistId));

            return result;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = AudioStreamingRoles.Artist)]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult> AddTracksToRelease([FromBody] AddTracksToReleaseDto payload)
        {
            var result = await _mediator.Send(new AddTracksToReleaseCommand(payload.Tracks, payload.ReleaseId));

            return result;
        }

        [HttpDelete("[action]/{trackId}")]
        [Authorize]
        public async Task<IApiResult> DeleteFavoriteTrack([FromRoute] int trackId)
        {
            var result = await _mediator.Send(new DeleteTrackFromFavoriteCommand(User.GetUserId(), trackId));

            return result;
        }

        [HttpDelete("[action]/{trackId}")]
        [Authorize]
        [CheckPlaylistPermissionFilter(PermissionType = PermissionType.Edit)]
        public async Task<IApiResult> DeleteTrackFromPlaylist([FromRoute] int trackId, [FromBody] PlaylistBaseDto payload)
        {
            var result = await _mediator.Send(new DeleteTrackFromPlaylistCommand(payload.PlaylistId, trackId));

            return result;
        }

        [HttpDelete("[action]/{trackId}")]
        [Authorize]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult> DeleteTrackFromRelease([FromRoute] int trackId, [FromBody] ReleaseBaseDto payload)
        {
            var result = await _mediator.Send(new DeleteTrackFromReleaseCommand(payload.ReleaseId, trackId));

            return result;
        }
    }
}
