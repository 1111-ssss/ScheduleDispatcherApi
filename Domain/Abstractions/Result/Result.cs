using System.Net.NetworkInformation;

namespace Domain.Abstractions.Result
{
    public interface IResultBase
    {
        bool IsSuccess { get; }
        ErrorCode? Error { get; }
        string? Message { get; }
    }
    public sealed class Result : IResultBase
    {
        public bool IsSuccess => Error is null;
        public ErrorCode? Error { get; }
        public string? Message { get; }
        internal Result(ErrorCode? errorCode = null, string? message = null)
        {
            Error = errorCode;
            Message = message;
        }
        public static Result Success() => new();
        public static Result Failed(ErrorCode errorCode, string? message) => new(errorCode, message);

        public static Result IsComplited() => Success();
    }
    public sealed class Result<T> : IResultBase
    {
        public Result(T value)
        {
            Value = value;
        }
        public T Value { get; }
        public bool IsSuccess => Error is null;
        public ErrorCode? Error { get; }
        public string? Message { get; }
        internal Result(T value, ErrorCode? error, string? message)
        {
            Value = value;
            Error = error ?? ErrorCode.Unknown;
            Message = message;
        }
        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failed(ErrorCode errorCode, string message) => new(default!, errorCode, message);
        public static implicit operator Result(Result<T> result) => result.IsSuccess ? Result.Success() : Result.Failed(result.Error!.Value, result.Message);

        public static Result<T> IsComplited(T value) => Success(value);
    }
}