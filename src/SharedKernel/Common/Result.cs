using SharedKernel.Enums;

namespace SharedKernel.Common;

public abstract class ResultBase
{
    public bool Succeeded { get; protected init; }
    public IEnumerable<Error> Errors { get; protected init; } = [];
    public string? Message { get; protected init; }
    public ResultCode Code { get; protected init; }
}

public class Result
    : ResultBase
{
    private Result()
    {
    }

    public static Result Success(
        string? message = null,
        ResultCode resultCode = ResultCode.Default)
        => new() { Succeeded = true, Message = message, Code = resultCode };
    
    public static Result Failure(
        Error error,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => Failure([error], resultCode, message);
    
    public static Result Failure(
        IEnumerable<Error> errors,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => new() { Succeeded = false, Code = resultCode, Errors = errors, Message = message };

    public static Result Forbidden(Error? error)
    {
        var baseError = new[] { new Error("forbidden request", ErrorCode.Forbidden) };
        var errors = error is null
            ? baseError
            : baseError.Prepend(error);

        return Failure(errors, ResultCode.Forbidden);
    }

    public static Result Handle(
        Result result,
        Error error,
        ResultCode resultCode = ResultCode.Default)
        => result.Succeeded
            ? result
            : Failure(result.Errors.Prepend(error), resultCode);
}

public class Result<T>
    : ResultBase
{
    public T Value
    {
        get => Succeeded
            ? field
            : throw new InvalidOperationException("Result did not succeed");
        init;
    } = default!;

    private Result()
    {
    }

    public static Result<T> Success(T value,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => new() { Succeeded = true, Value = value, Code = resultCode, Message = message };

    public static Result<T> Failure(
        IEnumerable<Error> errors,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => new() { Succeeded = false, Code = resultCode, Errors = errors, Message = message };

    public static Result<T> Failure(
        Error error,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => Failure([error], resultCode, message);

    public static Result<T> Forbidden(Error? error)
    {
        var baseError = new[] { new Error("forbidden request", ErrorCode.Forbidden) };
        var errors = error is null
            ? baseError
            : baseError.Prepend(error);

        return Failure(errors, ResultCode.Forbidden);
    }

    public static Result<T> Handle(
        Result<T> result,
        Error error,
        ResultCode resultCode = ResultCode.Default)
        => result.Succeeded
            ? result
            : Failure(result.Errors.Prepend(error), resultCode);
}