using Domain.Abstractions;

namespace Domain.Models.Habits;

public struct Identity: IStringSvo
{
    public Identity(string value)
    {
        Value = value;
    }

    public string Value { get; }
}