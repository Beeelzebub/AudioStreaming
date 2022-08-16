using AudioStreaming.Application.Abstractions.Responses;
using AudioStreaming.Application.DTOs.Genres;
using AudioStreaming.Application.Mediator.Genres.Commands;
using AudioStreaming.Application.Mediator.Genres.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AudioStreaming.Security;

namespace AudioStreaming.WebApi.Controllers
{
    public class GenreController : AudioStreamingController
    {
        public GenreController(IMediator mediator) : base(mediator) { }


        [HttpGet("[action]")]
        public async Task<IApiResult<ICollection<GenreDto>>> GetGenreList()
        {
            var result = await _mediator.Send(new GetGenreListQuery());

            return result;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = AudioStreamingRoles.Moderator)]
        public async Task<IApiResult<ICollection<string>>> AddGenres([FromBody] ICollection<GenreDto> payload)
        {
            var result = await _mediator.Send(new AddGenresCommand(payload));

            return result;
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = AudioStreamingRoles.Moderator)]
        public async Task<IApiResult> EditGenre([FromBody] GenreDto payload)
        {
            var result = await _mediator.Send(new EditGenreCommand(payload));

            return result;
        }

        [HttpDelete("[action]/{genreName}")]
        [Authorize(Roles = AudioStreamingRoles.Moderator)]
        public async Task<IApiResult> DeleteGenre([FromRoute] string genreName)
        {
            var result = await _mediator.Send(new DeleteGenreCommand(genreName));

            return result;
        }
    }
}
