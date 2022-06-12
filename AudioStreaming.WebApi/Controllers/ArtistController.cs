using MediatR;

namespace AudioStreaming.WebApi.Controllers
{
    public class ArtistController : AudioStreamingController
    {
        public ArtistController(IMediator mediator) : base(mediator) { }

    }
}
