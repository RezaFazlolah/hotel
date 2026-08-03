using Application.Auth.Commands;
using Domain.Models;

namespace Application.Auth.Factories;

public static class UserFactory
{
    /// <summary>
    /// maps RegisterCommand to User
    /// I didn't use AutoMapper, because I'm mapping to User which inherits from IdentityUser
    /// I have to ignore many properties
    /// this approach is cleaner
    /// </summary>
    /// <returns></returns>
    public static User CreateUserFromRegisterCommand(RegisterCommand registerCommand)
        => new()
        {
            PhoneNumber = registerCommand.PhoneNumber,
            FirstName = registerCommand.FirstName,
            LastName = registerCommand.LastName??string.Empty
        };
}