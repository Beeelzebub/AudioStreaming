using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Releases;
using AudioStreaming.Application.DTOs.Requests;
using AudioStreaming.Application.DTOs.Responses;
using AudioStreaming.Application.Mediator.Releases.Commands;
using AudioStreaming.Application.Mediator.Releases.Queries;
using AudioStreaming.Common.Extensions;
using AudioStreaming.Domain.Enums;
using AudioStreaming.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreaming.WebApi.Controllers
{
    public class ReleaseController : AudioStreamingController
    {
        public ReleaseController(IMediator mediator) : base(mediator) { }


        [HttpGet("[action]")]
        [Authorize(Roles = "Moderator")]
        public async Task<IApiResult<PagedList<ReleaseDto>>> GetAllReleasesWithDetails([FromQuery] RequestParameters parameters, 
            [FromQuery] string artistId = null,
            [FromBody] ICollection<ReleaseStage>? releaseStages = null)
        {
            var result = await _mediator.Send(new GetReleaseListQuery(parameters, releaseStages, artistId));
            
            return result;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IApiResult<PagedList<ReleaseDto>>> GetOwnReleasesWithDetails([FromQuery] RequestParameters parameters,
            [FromBody] ICollection<ReleaseStage> releaseStages)
        {
            var result = await _mediator.Send(new GetReleaseListQuery(parameters, null, User.GetUserId()));
           
            return result;
        }

        [HttpGet("[action]")]
        public async Task<IApiResult<PagedList<ReleaseDto>>> GetPublishedReleases([FromQuery] RequestParameters parameters, [FromQuery] string? artistId = null)
        {
            var result = await _mediator.Send(new GetReleaseListQuery(parameters, new List<ReleaseStage> { ReleaseStage.Published }, artistId));

            return result;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Artist")]
        public async Task<IApiResult<int>> CreateRelease([FromForm] CreateReleaseDto payload)
        {
            var result = await _mediator.Send(new CreateReleaseCommand(payload));

            return result;
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Artist")]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult> EditRelease([FromForm] EditReleaseDto payload)
        {
            var result = await _mediator.Send(new EditReleaseCommand(payload));

            return result;
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Artist")]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult> SendReleaseForVerification([FromBody] ReleaseBaseDto payload)
        {
            var result = await _mediator.Send(new SendReleaseForVerificationCommand(payload.ReleaseId));

            return result;
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Artist")]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult> PublishRelease([FromBody] PublishReleaseDto payload)
        {
            var result = await _mediator.Send(new PublishReleaseCommand(payload));

            return result;
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Artist")]
        [CheckAccessToReleaseFilter]
        public async Task<IApiResult> DeleteRelease([FromBody] ReleaseBaseDto payload)
        {
            var result = await _mediator.Send(new DeleteReleaseCommand(payload.ReleaseId));

            return result;
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Moderator")]
        public async Task<IApiResult> VerifiyRelease([FromBody] VerifyReleaseDto payload)
        {
            var result = await _mediator.Send(new VerifyReleaseCommand(payload));

            return result;
        }
    }
}
