using Domain.Abstractions.Result;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    where TResponse : class, IResultBase
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null);

                if (failures.Any())
                {
                    var errorMessage = failures.First().ErrorMessage;
                    
                    var responseType = typeof(TResponse);

                    if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                    {
                        var valueType = responseType.GenericTypeArguments[0];
                        var failedMethod = typeof(Result<>)
                            .MakeGenericType(valueType)
                            .GetMethod(nameof(Result<object>.Failed), new[] { typeof(ErrorCode), typeof(string) });

                        var failedResult = failedMethod!.Invoke(null, new object[] { ErrorCode.BadRequest, errorMessage });

                        return (TResponse)failedResult!;
                    }

                    return (TResponse)(object)Result.Failed(ErrorCode.BadRequest, errorMessage);
                }
            }

            return await next();
        }
    }
}