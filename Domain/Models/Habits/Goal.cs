using Domain.Abstractions;

namespace Domain.Models.Habits;

public struct Goal: IStringSvo
{
    public Goal(string value)
    {
        Value = value;
    }

    public string Value { get; }
}