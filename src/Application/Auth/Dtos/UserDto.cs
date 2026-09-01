using SharedKernel.Enums;

namespace Application.Auth.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public IReadOnlyList<UserRole> Roles { get; init; } = [];
}