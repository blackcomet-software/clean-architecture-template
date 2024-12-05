using Domain.Abstractions;
using Domain.Models.Users;
using FluentValidation;

namespace Domain.Models.Habits;

public class Habit : IEntity<HabitId>
{
    // Empty constructor needed for EF Core
    public Habit()
    {
    }

    public Habit(ActualHabit actualHabit, List<Goal> desiredGoals, Identity desiredIdentity,
        TimeAndOrLocation timeAndOrLocation, User user)
    {
        Id = HabitId.Next();
        ActualHabit = actualHabit;
        DesiredGoals = desiredGoals;
        DesiredIdentity = desiredIdentity;
        TimeAndOrLocation = timeAndOrLocation;
        User = user;
    }

    public HabitId Id { get; set; }
    public ActualHabit ActualHabit { get; set; }

    public User User { get; set; }
    
    public List<Goal> DesiredGoals { get; set; }

    public Identity DesiredIdentity { get; set; }

    public TimeAndOrLocation TimeAndOrLocation { get; set; }

    public string HabitSentence =>
        $"I Will {ActualHabit}, {TimeAndOrLocation} so that I can become {DesiredIdentity}.";

    public void Validate()
    {
        var validator = new HabitValidator();
        var result = validator.Validate(this);
        if (!result.IsValid) throw new ValidationException(result.Errors);
    }
}

public class HabitValidator : AbstractValidator<Habit>
{
    public HabitValidator()
    {
    }
}