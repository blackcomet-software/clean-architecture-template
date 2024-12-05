using System.Diagnostics.CodeAnalysis;
using Domain.Abstractions;

namespace Domain.Models.Habits;

public struct HabitId : IEntityId<HabitId>, IEntityIdStatic<HabitId>, IGuidSvo
{
    public Guid Value { get; }

    public HabitId(Guid value)
    {
        Value = value;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        FormattableString formattable = $"{Value}";
        return formattable.ToString(formatProvider);
    }

    public override string ToString()
    {
        return $"{Value}";
    }

    public bool Equals(HabitId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is HabitId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static HabitId Parse(string s, IFormatProvider? provider)
    {
        return new HabitId(Guid.Parse(s, provider));
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out HabitId result)
    {
        var succes = Guid.TryParse(s, provider, out var guid);
        result = new HabitId(guid);
        return succes;
    }

    public static HabitId Next()
    {
        return new HabitId(Guid.NewGuid());
    }

    public static HabitId Empty => new HabitId(Guid.Empty);
}