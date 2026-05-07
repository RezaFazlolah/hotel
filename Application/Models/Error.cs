namespace Application.Models;

public struct Error(string message)
{
    public string Message { get; set; } = message;
}