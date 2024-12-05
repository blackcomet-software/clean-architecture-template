using Application.Abstractions.Repositories;
using Domain.Models.Habits;

namespace Persistence.Repositories;

public class HabitRepository : Repository<Habit, HabitId>, IHabitRepository
{
    public HabitRepository(DbContext dbContext) : base(dbContext)
    {
    }
}