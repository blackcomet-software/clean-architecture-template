using System.Diagnostics.CodeAnalysis;
using Domain.Abstractions;

namespace Domain.Models.Users;

public struct UserId : IEntityId<UserId>, IEntityIdStatic<UserId>, IGuidSvo
{
    public Guid Value { get; }

    public UserId(Guid value)
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

    public bool Equals(UserId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is UserId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static UserId Parse(string s, IFormatProvider? provider)
    {
        return new UserId(Guid.Parse(s, provider));
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UserId result)
    {
        var succes = Guid.TryParse(s, provider, out var guid);
        result = new UserId(guid);
        return succes;
    }

    public static UserId Next()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Empty => new UserId(Guid.Empty);
}