using Application.Abstractions.Repositories;
using Application.MediatR.CQRS;
using Carter;
using Domain.Models.Habits;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Riok.Mapperly.Abstractions;

namespace Application.UseCases.Habits;

public class GetAllHabitsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("habits",
                async (ISender sender) =>
                {
                    return Results.Ok(await sender.Send(new GetAllHabitsQuery()));
                })
            .RequireAuthorization()
            .Produces<List<Habit>>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public record HabitDto
{
    public required HabitId Id { get; init; }
    
    public required ActualHabit ActualHabit { get; init; }
    
    public required List<Goal> DesiredGoals { get; init; }

    public required Identity DesiredIdentity { get; init; }

    public required TimeAndOrLocation TimeAndOrLocation { get; init; }
}

[Mapper]
public partial class HabitDtoMapper
{
    [MapperIgnoreSource(nameof(Habit.User))]
    [MapperIgnoreSource(nameof(Habit.HabitSentence))]
    public partial HabitDto Map(Habit habit);
}

public record GetAllHabitsQuery : UserQuery<List<HabitDto>>;

public class GetAllHabitsHandler : IQueryHandler<GetAllHabitsQuery, List<HabitDto>>
{
    private readonly IHabitRepository _repository;

    public GetAllHabitsHandler(IHabitRepository repository)
    {
        _repository = repository;
    }


    public async Task<List<HabitDto>> Handle(GetAllHabitsQuery query, CancellationToken cancellationToken)
    {
        var habits = await _repository.GetAllAsync();
        var mapper = new HabitDtoMapper();
        return habits.Select(mapper.Map).ToList();
    }
}