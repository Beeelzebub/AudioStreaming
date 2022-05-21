using MusicStreaming.Application.Abstractions.Responses;
using MediatR;

namespace MusicStreaming.Application.Mediator.Common.Queries
{
    public interface IQuery<TResult> : IRequest<IApiResult<TResult>>
    {

    }
}
