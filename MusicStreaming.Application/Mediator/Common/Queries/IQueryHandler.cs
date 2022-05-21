using MediatR;
using MusicStreaming.Application.Abstractions.Responses;

namespace MusicStreaming.Application.Mediator.Common.Queries
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, IApiResult<TResponse>>
        where TRequest : IQuery<TResponse>
    {

    }
}
