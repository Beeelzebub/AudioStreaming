using MediatR;
using MusicStreaming.Application.Abstractions.Response;

namespace MusicStreaming.Application.Mediator.Common.Queries
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, IApiResult<TResponse>>
        where TRequest : IQuery<TResponse>
    {

    }
}
