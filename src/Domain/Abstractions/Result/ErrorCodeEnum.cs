using Domain.Model.Result;

namespace Domain.Abstractions.Result;

public enum ErrorCode
{
    Unknown = 0,

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
    [HttpStatusCode(400)]
    InvalidWorkloadSummary,
    [HttpStatusCode(400)]
    InvalidWorkload,
    [HttpStatusCode(400)]
    InvalidRemoval,
    [HttpStatusCode(400)]
    InvalidLessonName,
    [HttpStatusCode(400)]
    InvalidTeacherName,
    [HttpStatusCode(400)]
    InvalidSemester,

    [HttpStatusCode(401)]
    Unauthorized,
    [HttpStatusCode(401)]
    TokenExpired,
    [HttpStatusCode(401)]
    InvalidToken,

    [HttpStatusCode(403)]
    Forbidden,

    [HttpStatusCode(404)]
    NotFound,
    [HttpStatusCode(404)]
    UserNotFound,
    [HttpStatusCode(404)]
    TeacherNotFound,

    [HttpStatusCode(409)]
    Conflict,
    [HttpStatusCode(409)]
    UserAlreadyExists,

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
