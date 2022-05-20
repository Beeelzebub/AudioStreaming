using MusicStreaming.Application.Abstractions.Response;
using MediatR;

namespace MusicStreaming.Application.Mediator.Common.Queries
{
    public interface IQuery<TResult> : IRequest<IApiResult<TResult>>
    {

    }
}
