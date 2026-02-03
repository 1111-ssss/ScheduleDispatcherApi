using Microsoft.AspNetCore.Mvc;
using Domain.Abstractions.Result;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this IResultBase result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(null);
            
        if (!result.Error.HasValue)
            return new BadRequestObjectResult(new { error = result.Message ?? "NoErrorCode" });

        int statusCode = HttpStatusCodeAttribute.GetHttpStatusCode(result.Error.Value);
        string statusCodeName = Enum.GetName(typeof(ErrorCode), result.Error.Value) ?? "Unknown";
        string? resultMessage = result.Message;

        return statusCode switch
        {
            400 => new BadRequestObjectResult(new { error = statusCodeName, message = resultMessage }),
            401 => new UnauthorizedObjectResult(new { error = statusCodeName, message = resultMessage }),
            404 => new NotFoundObjectResult(new { error = statusCodeName, message = resultMessage }),
            409 => new ConflictObjectResult(new { error = statusCodeName, message = resultMessage }),
            _ => new ObjectResult(new { error = resultMessage }) { StatusCode = statusCode },
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return ((IResultBase)result).ToActionResult();
    }
}