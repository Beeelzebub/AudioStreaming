using AudioStreaming.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreaming.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class AudioStreamingController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public AudioStreamingController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
