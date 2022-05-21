using MusicStreaming.Application.Abstractions.Responses;
using MediatR;

namespace MusicStreaming.Application.Mediator.Common.Commands
{
{
    public interface ICommand<TResult> : IRequest<IApiResult<TResult>>
    {

    }
}
