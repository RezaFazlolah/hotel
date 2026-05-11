namespace Application.Models;

public struct Error(string message)
{
    public string Message { get; init; } = message;
}