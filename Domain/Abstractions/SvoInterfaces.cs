namespace Domain.Abstractions;

public interface IStringSvo
{
    public string Value { get; }
}

public interface IGuidSvo
{
    public Guid Value { get; }
}

public interface IIntSvo
{
    public int Value { get; }
}

public static class SvoInterfaces
{
    public static List<Type> Types =
    [
        typeof(IStringSvo),
        typeof(IGuidSvo),
        typeof(IIntSvo)
    ];
}