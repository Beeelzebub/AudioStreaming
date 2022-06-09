using MediatR;
using AudioStreaming.Application.Abstractions.Responses;

namespace AudioStreaming.Application.Mediator.Common.Queries
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, IApiResult<TResponse>>
        where TRequest : IQuery<TResponse>
    {

    }
}
