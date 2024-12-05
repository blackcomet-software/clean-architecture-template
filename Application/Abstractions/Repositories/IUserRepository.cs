

using Domain.Models;
using Domain.Models.Users;

namespace Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
    User GetOrCreate(UserId userId);
}