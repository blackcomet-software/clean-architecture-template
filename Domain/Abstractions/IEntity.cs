namespace Domain.Abstractions;

public interface IEntity<TEntityId> : IValidatable
where TEntityId : struct, IEntityId<TEntityId>
{
    public TEntityId Id { get; }
}

public interface IValidatable
{
    void Validate();
}