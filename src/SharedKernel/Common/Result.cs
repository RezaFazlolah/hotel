using SharedKernel.Enums;

namespace SharedKernel.Common;

public abstract class BaseResult
{
    public bool Succeeded { get; init; }
    public IEnumerable<Error> Errors { get; init; } = [];
    public string? Message { get; init; }
    public ResultCode Code { get; init; }
}

public class Result
    : BaseResult
{
    private Result()
    {
    }

    public static Result Success(
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => new() { Succeeded = true, Code = resultCode, Message = message };

    public static Result Failure(
        IEnumerable<Error> errors,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => new() { Succeeded = false, Code = resultCode, Errors = errors, Message = message };

    public static Result Failure(
        Error error,
        ResultCode resultCode = ResultCode.Default,
        string? message = null)
        => Failure([error], resultCode, message);

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
    : BaseResult
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