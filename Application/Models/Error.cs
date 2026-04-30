namespace Application.Models;

public struct Error
{
    public string Message { get; set; }

    public Error(string message)
        => Message = message;
}