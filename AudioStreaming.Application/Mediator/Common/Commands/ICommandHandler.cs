using MediatR;
using AudioStreaming.Application.Abstractions.Responses;

namespace AudioStreaming.Application.Mediator.Common.Commands
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, IApiResult<TResponse>>
        where TRequest : ICommand<TResponse>
    {

    }
}
