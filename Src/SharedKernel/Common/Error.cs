using SharedKernel.Enums;

namespace SharedKernel.Common;

public class Error(string message, ErrorCode code = ErrorCode.Default, Error? innerError = null)
{
    public string Message { get; init; } = message;
    public ErrorCode Code { get; init; } = code;
    public Error? InnerError { get; init; } = innerError;
    
    public override string ToString()
        => $"{Code.ToString()}: {Message}{Environment.NewLine}{InnerError?.ToString(1) ?? string.Empty}";

    private string ToString(int tab)
    // this method is only used for properly indenting inner error
    {
        var result=string.Empty;
        for(var i = 0; i < tab; i++)
            result += "\t";
        return result + $"{Code.ToString()}: {Message}{Environment.NewLine}{InnerError?.ToString(tab + 1) ?? string.Empty}";
    }
}