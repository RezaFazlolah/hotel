namespace Application.Interfaces.ServiceInterfaces;

public interface ICurrentUserService
{
    Guid? CurrentUserId { get; }
}