using SharedKernel.Enums;

namespace SharedKernel.Common;

public class Error(
    string message,
    ErrorCode code = ErrorCode.Default,
    Error? innerError = null)
{
    public string Message => message;

    public override string ToString()
        => $"{code.ToString()}: {Message}{innerError?.ToString(1) ?? string.Empty}";

    // proper indentation of inner error
    private string ToString(int tab)
    {
        var result = Environment.NewLine;
        for (var i = 0; i < tab; i++)
            result += "\t";
        return result + $"{code.ToString()}: {Message}{innerError?.ToString(tab + 1) ?? string.Empty}";
    }
}