using System.Reflection;
using Domain.Abstractions;
using Domain.Models.Habits;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.EFCoreSerialization;

namespace Persistence;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DbContext> _logger;

    public DbContext(DbContextOptions<DbContext> options, IConfiguration configuration,
        ILogger<DbContext> logger) :
        base(options)
    {
        _configuration = configuration;
        _logger = logger;
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(
            _configuration.GetConnectionString("Development"));
        optionsBuilder.LogTo(information => _logger.LogInformation(information));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        AddSvoConverters(configurationBuilder);
    }

    private void AddSvoConverters(ModelConfigurationBuilder configurationBuilder)
    {
        var domain = Assembly.GetAssembly(typeof(UserId));

        var svoTypes = domain!.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => SvoInterfaces.Types.Contains(i)))
            .ToList();

        foreach (var type in svoTypes)
        {
            var converterType = type switch
            {
                _ when typeof(IStringSvo).IsAssignableFrom(type) =>
                    typeof(SvoToStringEfConverter<>).MakeGenericType(type),
                _ when typeof(IGuidSvo).IsAssignableFrom(type) =>
                    typeof(SvoToGuidEfConverter<>).MakeGenericType(type),
                _ when typeof(IIntSvo).IsAssignableFrom(type) =>
                    typeof(SvoToIntEfConverter<>).MakeGenericType(type),
                _ => throw new Exception("No EF Core converter found for svo type."),
            };
            
            var enumerableConverterType = type switch
            {
                _ when typeof(IStringSvo).IsAssignableFrom(type) =>
                    typeof(SvoToStringEfListConverter<>).MakeGenericType(type),
                _ when typeof(IGuidSvo).IsAssignableFrom(type) =>
                    typeof(SvoToGuidEfListConverter<>).MakeGenericType(type),
                _ when typeof(IIntSvo).IsAssignableFrom(type) =>
                    typeof(SvoToIntEfListConverter<>).MakeGenericType(type),
                _ => throw new Exception("No EF Core converter found for svo type."),
            };

            configurationBuilder.Properties(typeof(List<>).MakeGenericType(type)).HaveConversion(enumerableConverterType);
            configurationBuilder.Properties(type).HaveConversion(converterType);
        }
    }
}