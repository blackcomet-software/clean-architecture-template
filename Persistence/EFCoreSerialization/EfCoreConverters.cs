using Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.EFCoreSerialization;

public class SvoToGuidEfConverter<T> : ValueConverter<T, Guid>
    where T : struct, IGuidSvo
{
    public SvoToGuidEfConverter() : base(v => v.Value, v => (T)Activator.CreateInstance(typeof(T), v)!)
    {
    }
}

public class SvoToGuidEfListConverter<T> : ValueConverter<List<T>, List<Guid>>
    where T : struct, IGuidSvo
{
    public SvoToGuidEfListConverter() : base(list => list.Select(x => x.Value).ToList(),
        list => list.Select(x => (T)Activator.CreateInstance(typeof(T), x)!).ToList())
    {
    }
}

public class SvoToStringEfConverter<T> : ValueConverter<T, string>
    where T : struct, IStringSvo
{
    public SvoToStringEfConverter() : base(v => v.Value, v => (T)Activator.CreateInstance(typeof(T), v)!)
    {
    }
}

public class SvoToStringEfListConverter<T> : ValueConverter<List<T>, List<string>>
    where T : struct, IStringSvo
{
    public SvoToStringEfListConverter() : base(list => list.Select(x => x.Value).ToList(),
        list => list.Select(x => (T)Activator.CreateInstance(typeof(T), x)!).ToList())
    {
    }
}

public class SvoToIntEfConverter<T> : ValueConverter<T, int>
    where T : struct, IIntSvo
{
    public SvoToIntEfConverter() : base(v => v.Value, v => (T)Activator.CreateInstance(typeof(T), v)!)
    {
    }
}

public class SvoToIntEfListConverter<T> : ValueConverter<List<T>, List<int>>
    where T : struct, IIntSvo
{
    public SvoToIntEfListConverter() : base(list => list.Select(x => x.Value).ToList(),
        list => list.Select(x => (T)Activator.CreateInstance(typeof(T), x)!).ToList())
    {
    }
}