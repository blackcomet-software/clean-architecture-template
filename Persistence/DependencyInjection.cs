using Application.Abstractions;
using Application.Abstractions.Repositories;
using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistanceProject(this IServiceCollection services)
    {
        services.AddDbContext<DbContext>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IHabitRepository, HabitRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        return services;
    }
}