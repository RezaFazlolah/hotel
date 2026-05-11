namespace SharedKernel.Common;

public struct Error(string message)
{
    public string Message { get; init; } = message;
}