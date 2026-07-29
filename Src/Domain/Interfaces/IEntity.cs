namespace Domain.Interfaces;

public interface IEntity<TId>
    where TId : IEquatable<TId>, new()
{
    public TId Id { get; }
}