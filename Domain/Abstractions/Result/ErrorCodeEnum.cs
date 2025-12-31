namespace Domain.Abstractions.Result
{
    public enum ErrorCode
    {
        Unknown = 0,

        // дефолтные коды ошибок
        BadRequest = 400,
        ValidationFailed = 422,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        TooManyRequests = 429,
        InternalServerError = 500,
        ServiceUnavailable = 503,

        // кастомные коды ошибок
        // InvalidUsernameOrPassword = 1001,
        // TokenExpired = 1002,
        // UserNotFound = 1003,
        // UserAlreadyExists = 1004,
    }
}