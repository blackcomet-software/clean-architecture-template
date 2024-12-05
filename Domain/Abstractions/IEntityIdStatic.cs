namespace Domain.Abstractions;

public interface IEntityIdStatic<TEntityId> : IParsable<TEntityId>
where TEntityId : struct, IEntityId<TEntityId>, IParsable<TEntityId>
{
    public static abstract TEntityId Next();
    public static abstract TEntityId Empty { get; }
}