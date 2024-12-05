using Domain.Models.Habits;

namespace Application.Abstractions.Repositories;

public interface IHabitRepository : IRepository<Habit, HabitId>
{
}