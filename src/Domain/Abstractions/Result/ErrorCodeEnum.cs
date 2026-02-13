namespace Domain.Abstractions.Result;

public enum ErrorCode
{
    Unknown = 0,

    //Ошибки валидации (400 Bad Request)
    [HttpStatusCode(400)]
    BadRequest,
    [HttpStatusCode(400)]
    InvalidPassword,
    [HttpStatusCode(400)]
    InvalidUsername,
    [HttpStatusCode(400)]
    InvalidUsernameOrPassword,
    [HttpStatusCode(400)]
    InvalidPagination,
    [HttpStatusCode(400)]
    InvalidGroupName,
    [HttpStatusCode(400)]
    InvalidDate,

    //Ошибки авторизации (401 Unauthorized)
    [HttpStatusCode(401)]
    Unauthorized,
    [HttpStatusCode(401)]
    TokenExpired,
    [HttpStatusCode(401)]
    InvalidToken,

    //Нет доступа (403 Forbidden)
    [HttpStatusCode(403)]
    Forbidden,

    //Не найден (404 Not Found)
    [HttpStatusCode(404)]
    NotFound,
    [HttpStatusCode(404)]
    UserNotFound,

    //Ошибки конфликтации (409 Conflict)
    [HttpStatusCode(409)]
    Conflict,
    [HttpStatusCode(409)]
    UserAlreadyExists,

    //Ошибка сервера (500 Internal Server Error)
    [HttpStatusCode(500)]
    InternalServerError,
    [HttpStatusCode(500)]
    DatabaseError,
    [HttpStatusCode(500)]
    UnknownError,
    [HttpStatusCode(500)]
    TokenGenerationError,
    [HttpStatusCode(500)]
    NoPasswordHash,
}