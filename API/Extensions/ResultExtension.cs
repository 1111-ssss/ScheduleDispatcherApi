using Microsoft.AspNetCore.Mvc;
using Domain.Abstractions.Result;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this IResultBase result)
    {
        if (!result.Error.HasValue)
            return new BadRequestObjectResult(new { error = result.Message ?? "Bad request" });

        int code = (int)result.Error;

        return code switch
        {
            400 => new BadRequestObjectResult(new { error = result.Message ?? "Bad request", code }),
            401 => new UnauthorizedObjectResult(new { error = result.Message ?? "Unauthorized", code }),
            404 => new NotFoundObjectResult(new { error = result.Message ?? "Not found", code }),
            409 => new ConflictObjectResult(new { error = result.Message ?? "Conflict", code }),
            _ => new ObjectResult(new { error = result.Message, code }) { StatusCode = code },
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return ((IResultBase)result).ToActionResult();
    }
}