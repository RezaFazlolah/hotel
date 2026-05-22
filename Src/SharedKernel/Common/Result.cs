using SharedKernel.Enums;

namespace SharedKernel.Common;

public class Result<T>
{
    public bool Succeeded { get; init; }
    public T Value { get; init; }
    public IEnumerable<Error> Errors { get; init; } = [];
    public string? Message { get; init; }
    public ResultCode Code { get; init; }

    public static Result<T> Success(T value, ResultCode resultCode = ResultCode.Default, string? message=null) => new() { Succeeded = true, Value = value, Code = resultCode, Message = message };

    public static Result<T> Failure(IEnumerable<Error> errors, ResultCode resultCode = Enums.ResultCode.Default, string? message = null) =>
        new() { Succeeded = false, Code = resultCode, Errors = errors, Message = message };

    public static Result<T> Failure(Error error, ResultCode resultCode = Enums.ResultCode.Default, string? message = null)
        => Failure([error], resultCode, message);
}