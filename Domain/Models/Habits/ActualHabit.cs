using Domain.Abstractions;

namespace Domain.Models.Habits;

public struct ActualHabit : IStringSvo
{
    public string Value { get; }

    public ActualHabit(string value)
    {
        Value = value;
    }
}