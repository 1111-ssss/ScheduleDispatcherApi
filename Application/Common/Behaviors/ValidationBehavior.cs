using Domain.Abstractions.Result;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : class, IResultBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (!failures.Any())
            return await next();

        Dictionary<string, string> details = new();
        failures.ForEach(f => details.Add(f.CustomState?.ToString() ?? "Unknown", f.ErrorMessage));

        var mainError = failures.First();
        var errorCode = mainError.CustomState as ErrorCode?
            ?? ErrorCode.BadRequest;

        string errorMessage = mainError.ErrorMessage;

        if (typeof(TResponse).IsGenericType && 
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            return (TResponse)typeof(TResponse)
                .GetMethod(nameof(Result.Failed))!
                .Invoke(null, new object[] { errorCode, errorMessage, details })!;
        }

        return (TResponse)(object)Result.Failed(errorCode, errorMessage, details);
    }
}