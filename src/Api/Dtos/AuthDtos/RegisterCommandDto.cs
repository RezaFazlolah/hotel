namespace Api.Dtos.AuthDtos;

public record RegisterCommandDto
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public string? LastName { get; init; }
}