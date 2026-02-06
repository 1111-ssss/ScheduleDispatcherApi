namespace Domain.Abstractions.Result;

public interface IResultBase
{
    bool IsSuccess { get; }
    bool IsComplited { get; }
    ErrorCode? Error { get; }
    string? Message { get; }
    Dictionary<string, string>? Details { get; }
}
public sealed class Result : IResultBase
{
    public bool IsSuccess => Error is null;
    public bool IsComplited => IsSuccess;
    public ErrorCode? Error { get; }
    public string? Message { get; }
    public Dictionary<string, string>? Details { get; }
    internal Result(
        ErrorCode? errorCode = null,
        string? message = null,
        Dictionary<string, string>? details = null
    )
    {
        Error = errorCode;
        Message = message;
        Details = details;
    }
    public static Result Success() => new();
    public static Result Failed(
        ErrorCode errorCode,
        string? message = null,
        Dictionary<string, string>? details = null
    ) => new(errorCode, message, details);

    public static Result CompletedOperation() => Success();
    public static Result FailedOperation(
        ErrorCode errorCode,
        string? message = null,
        Dictionary<string, string>? details = null
    ) => Failed(errorCode, message, details);
}
public sealed class Result<T> : IResultBase
{
    public T Value { get; }
    public bool IsSuccess => Error is null;
    public bool IsComplited => IsSuccess;
    public ErrorCode? Error { get; }
    public string? Message { get; }
    public Dictionary<string, string>? Details { get; }
    internal Result(
        T value,
        ErrorCode? error,
        string? message,
        Dictionary<string, string>? details = null
    )
    {
        Value = value;
        Error = error;
        Message = message;
        Details = details;
    }
    public static Result<T> Success(T value) => new(value, null, null, null);
    public static Result<T> Failed(
        ErrorCode errorCode,
        string? message = null,
        Dictionary<string, string>? details = null
    ) => new(default!, errorCode, message, details);

    public static implicit operator Result(Result<T> result) => new(result.Error, result.Message, result.Details);
    public static implicit operator Result<T>(Result result) => new(default!, result.Error, result.Message, result.Details);

    public static Result<T> CompletedOperation(T value) => Success(value);
    public static Result<T> FailedOperation(ErrorCode errorCode, string? message = null, Dictionary<string, string>? details = null) => Failed(errorCode, message, details);
}