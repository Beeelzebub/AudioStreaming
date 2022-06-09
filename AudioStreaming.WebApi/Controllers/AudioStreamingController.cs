using AudioStreaming.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreaming.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    public class AudioStreamingController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly string? _userId;

        public AudioStreamingController(IMediator mediator)
        {
            _mediator = mediator;
            _userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }
    }
}
