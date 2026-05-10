namespace Application.Interfaces;

public interface ICurrentUserService
{
    Guid? CurrentUserId { get; }
}