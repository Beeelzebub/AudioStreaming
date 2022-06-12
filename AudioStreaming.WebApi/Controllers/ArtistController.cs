using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Artists;
using AudioStreaming.Application.Mediator.Artists.Commands;
using AudioStreaming.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreaming.WebApi.Controllers
{
    public class ArtistController : AudioStreamingController
    {
        public ArtistController(IMediator mediator) : base(mediator) { }


        [HttpPost("[action]")]
        [Authorize]
        public async Task<IApiResult> CreateArtist([FromBody] CreateArtistDto payload)
        {
            var result = await _mediator.Send(new CreateArtistCommand(payload, User.GetUserId()));

            return result;
        }

        [HttpPatch("[action]/{artistId}")]
        //[Authorize(Roles = "Moderator")]
        public async Task<IApiResult> ConfirmArtist([FromRoute] string artistId)
        {
            var result = await _mediator.Send(new ConfirmArtistCommand(artistId));

            return result;
        }
    }
}
