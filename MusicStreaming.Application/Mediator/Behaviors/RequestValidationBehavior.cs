using MusicStreaming.Application.Abstractions.Response;
using FluentValidation;
using MediatR;

namespace MusicStreaming.Application.Mediator.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IApiResult, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Select(e => e.ErrorMessage)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var response = new TResponse();
                response.IsSuccess = false;
                response.Errors = failures;

                return Task.FromResult(response);
            }

            return next();
        }
    }
}
