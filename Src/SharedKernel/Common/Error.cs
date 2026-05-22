using SharedKernel.Enums;

namespace SharedKernel.Common;

public struct Error(string message, ErrorCode code = ErrorCode.Default)
{
    public string Message { get; init; } = message;
    public ErrorCode Code { get; init; } = code;
    // public Error? InnerError { get; init; } = new();
}