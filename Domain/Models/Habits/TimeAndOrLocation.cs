using Domain.Abstractions;

namespace Domain.Models.Habits;

public struct TimeAndOrLocation: IStringSvo
{
    public string Value { get; }

    public TimeAndOrLocation(string value)
    {
        Value = value;
    }
}