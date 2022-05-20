using MediatR;
using MusicStreaming.Application.Abstractions.Response;

namespace MusicStreaming.Application.Mediator.Common.Commands
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, IApiResult<TResponse>>
        where TRequest : ICommand<TResponse>
    {

    }
}
