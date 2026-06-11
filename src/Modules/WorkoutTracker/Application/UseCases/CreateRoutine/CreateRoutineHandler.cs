// Path: src/Modules/WorkoutTracker/Application/UseCases/CreateRoutine/CreateRoutineHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.CreateRoutine;
public sealed class CreateRoutineHandler(IRoutineRepository repository, ILogger<CreateRoutineHandler> logger)
    : IRequestHandler<CreateRoutineCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(CreateRoutineCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Error.Validation("Routine.InvalidName", "Name cannot be empty.");
        var routine = new Routine
        {
            UserId = request.UserId, Name = request.Name, Description = request.Description,
            Category = request.Category, DaysOfWeek = request.DaysOfWeek,
            Exercises = request.Exercises.Select(e => new Exercise {
                Id = Guid.NewGuid().ToString(), Name = e.Name, MuscleGroup = e.MuscleGroup,
                Equipment = e.Equipment, Sets = e.Sets, Reps = e.Reps, WeightKg = e.WeightKg,
                RestSeconds = e.RestSeconds, Notes = e.Notes, ExternalId = e.ExternalId, IsImported = e.IsImported
            }).ToList(),
            CreatedAt = DateTime.UtcNow
        };
        var id = await repository.CreateAsync(routine);
        logger.LogInformation("Routine {Id} created for user {UserId}", id, request.UserId);
        return id;
    }
}