using AudioStreaming.Application.Abstractions.Responses;
using MediatR;

namespace AudioStreaming.Application.Mediator.Common.Commands
{
    public interface ICommand<TResult> : IRequest<IApiResult<TResult>>
    {

    }
}
