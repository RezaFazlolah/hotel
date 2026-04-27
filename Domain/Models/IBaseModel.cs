namespace Domain.Models;

public interface IBaseModel<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}