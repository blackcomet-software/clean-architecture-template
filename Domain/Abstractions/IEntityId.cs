namespace Domain.Abstractions;

public interface IEntityId<TEntityId> : IEquatable<TEntityId>, IFormattable
{
    public Guid Value { get; }
}