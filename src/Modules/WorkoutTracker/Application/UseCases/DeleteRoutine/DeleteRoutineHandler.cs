// Path: src/Modules/WorkoutTracker/Application/UseCases/DeleteRoutine/DeleteRoutineHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.DeleteRoutine;
public sealed class DeleteRoutineHandler(IRoutineRepository repository)
    : IRequestHandler<DeleteRoutineCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteRoutineCommand request, CancellationToken ct)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Routine.NotFound", "Routine not found.");
        await repository.DeleteAsync(request.Id, request.UserId);
        return Result.Deleted;
    }
}