using AudioStreaming.Application.Abstractions.Responses;
using MediatR;

namespace AudioStreaming.Application.Mediator.Common.Queries
{
    public interface IQuery<TResult> : IRequest<IApiResult<TResult>>
    {

    }
}
