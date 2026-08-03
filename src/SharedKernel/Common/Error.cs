using SharedKernel.Enums;

namespace SharedKernel.Common;

public class Error(string message, ErrorCode code = ErrorCode.Default, Error? innerError = null)
{
    public string Message { get; init; } = message;
    public ErrorCode Code { get; init; } = code;
    public Error? InnerError { get; init; } = innerError;
    
    public override string ToString()
        => $"{Code.ToString()}: {Message}{InnerError?.ToString(1) ?? string.Empty}";

    private string ToString(int tab)
    // this method is only used for proper indentation of inner error
    {
        var result=Environment.NewLine;
        for(var i = 0; i < tab; i++)
            result += "\t";
        return result + $"{Code.ToString()}: {Message}{InnerError?.ToString(tab + 1) ?? string.Empty}";
    }
}