using Domain.Abstractions;
using Domain.Models.Habits;
using FluentValidation;

namespace Domain.Models.Users;

public class User : IEntity<UserId>
{
    private readonly UserValidator _validator = new();

    // Empty constructor needed for EF Core
    private User()
    {
    }

    private User(UserId userId)
    {
        Id = userId;
        Habits = [];
    }

    public List<Habit> Habits { get; set; }

    public static User Create(UserId userId)
    {
        return new User(userId);
    }

    public UserId Id { get; set; }

    public void Validate()
    {
        var validator = new UserValidator();
        var result = validator.Validate(this);
        if (!result.IsValid) throw new ValidationException(result.Errors);
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
    }
}