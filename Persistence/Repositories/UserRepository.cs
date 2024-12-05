using Application.Abstractions.Repositories;
using Domain.Models;
using Domain.Models.Users;

namespace Persistence.Repositories;

public class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(DbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Gets a user from the database or creates one if it doesn't exist yet.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns> Returns true if a new user is created. </returns>
    public User GetOrCreate(UserId userId)
    {
        var existingUser = DbContext.Users.FirstOrDefault(x => x.Id.Equals(userId));
        
        if (existingUser is null)
        {
            var newUser = User.Create(userId);
            DbContext.Users.Add(newUser);
            return newUser;
        }

        return existingUser;
    }
}