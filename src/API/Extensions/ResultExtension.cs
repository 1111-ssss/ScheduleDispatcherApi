using Domain.Abstractions.Result;
using Domain.Model.Result;

public static class ResultExtensions
{
    public static IResult ToApiResult(this IResultBase result)
    {
        if (!result.Error.HasValue)
            return Results.BadRequest(new { message = result.Message ?? "Unknown error" });

        int statusCode = HttpStatusCodeAttribute.GetHttpStatusCode(result.Error.Value);
        var body = new
        {
            error = Enum.GetName(typeof(ErrorCode), result.Error.Value) ?? "Unknown",
            message = result.Message,
            details = result.Details
        };

        return statusCode switch
        {
            400 => Results.BadRequest(body),
            401 => Results.Json(body, statusCode: statusCode),
            403 => Results.Json(body, statusCode: statusCode),
            404 => Results.NotFound(body),
            409 => Results.Conflict(body),
            _ => Results.Json(body, statusCode: statusCode),
        };
    }

    public static IResult ToApiResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return ((IResultBase)result).ToApiResult();
    }
}