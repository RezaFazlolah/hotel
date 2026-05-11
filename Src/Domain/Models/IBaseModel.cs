namespace Domain.Models;

public interface IBaseModel<TId>
    where TId : IEquatable<TId>, new()
{
    public TId Id { get; set; }
}