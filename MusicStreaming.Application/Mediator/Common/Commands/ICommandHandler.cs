using MediatR;
using MusicStreaming.Application.Abstractions.Responses;

namespace MusicStreaming.Application.Mediator.Common.Commands
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, IApiResult<TResponse>>
        where TRequest : ICommand<TResponse>
    {

    }
}
