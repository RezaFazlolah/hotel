namespace Application.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public IEnumerable<Error> Errors { get; set; } = [];
    public int Code { get; set; }

    public static Result<T> Success(T? value, int code = 200) => new() { IsSuccess = true, Value = value, Code = code };

    public static Result<T> Failure(IEnumerable<Error> errors, int code) =>
        new() { IsSuccess = false, Code = code, Errors = errors };

    public static Result<T> Failure(Error error, int code)
        => Failure([error], code);
}