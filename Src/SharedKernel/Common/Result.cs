namespace SharedKernel.Common;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T Value { get; init; }
    public IEnumerable<Error> Errors { get; init; } = [];
    public int Code { get; init; }

    public static Result<T> Success(T? value, int code = 200) => new() { IsSuccess = true, Value = value, Code = code };

    public static Result<T> Failure(IEnumerable<Error> errors, int code) =>
        new() { IsSuccess = false, Code = code, Errors = errors };

    public static Result<T> Failure(Error error, int code)
        => Failure([error], code);
}