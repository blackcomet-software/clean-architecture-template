using Application.Abstractions;
using Application.Abstractions.Repositories;
using Application.MediatR.CQRS;
using Carter;
using Domain.Abstractions;
using Domain.Models.Habits;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.UseCases.Habits;

public class CreateHabitEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("habits",
                async (CreateHabitCommand command, ISender sender) =>
                {
                    return Results.Ok(await sender.Send(command));
                })
            .RequireAuthorization()
            .Produces<HabitId>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
public record CreateHabitCommand : UserCommand<HabitId>
{
    public required ActualHabit ActualHabit { get; init; }
    public required List<Goal> DesiredGoals { get; init; }
    public required Identity DesiredIdentity { get; init; }
    public required TimeAndOrLocation TimeAndOrLocation { get; init; }
}

public class CreateHabitHandler : ICommandHandler<CreateHabitCommand, HabitId>
{
    private readonly IHabitRepository _habitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHabitHandler(IHabitRepository habitRepository, IUnitOfWork unitOfWork)
    {
        _habitRepository = habitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<HabitId> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = new Habit(request.ActualHabit, request.DesiredGoals, request.DesiredIdentity, request.TimeAndOrLocation, request.CurrentUser!);
        
        await _habitRepository.AddAsync(habit);
        await _unitOfWork.SaveChangesAsync();

        return habit.Id;
    }
}

public class CreateHabitCommandValidator : AbstractValidator<CreateHabitCommand>
{
    public CreateHabitCommandValidator()
    {
    }
}