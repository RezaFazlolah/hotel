namespace Domain.Enums;

public enum UserRole
{
    Guest = 0,
    Manager = 1,
    Admin = 2
}

// public static class UserRole
// {
//     public const string Guest = "Guest";
//     public const string Manager = "Manager";
//     public const string Admin = "Admin";
//
//     public static IEnumerable<string> GetAll()
//         => [Guest, Manager, Admin];
// }