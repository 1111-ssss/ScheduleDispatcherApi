using Domain.Abstractions.Result;

public interface IResultBase
{
    bool IsSuccess { get; }
    bool IsComplited { get; }
    ErrorCode? Error { get; }
    string? Message { get; }
    Dictionary<string, string>? Details { get; }
}