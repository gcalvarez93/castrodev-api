// Path: src/Modules/WorkoutTracker/Application/UseCases/LogBodyWeight/LogBodyWeightHandler.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Domain.Entities;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.LogBodyWeight;
public sealed class LogBodyWeightHandler(IBodyWeightRepository repository, ILogger<LogBodyWeightHandler> logger)
    : IRequestHandler<LogBodyWeightCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(LogBodyWeightCommand request, CancellationToken ct)
    {
        if (request.WeightKg <= 0)
            return Error.Validation("BodyWeight.Invalid", "Weight must be greater than zero.");
        var entry = new BodyWeight {
            UserId = request.UserId, WeightKg = request.WeightKg,
            Date = request.Date, Notes = request.Notes, CreatedAt = DateTime.UtcNow
        };
        var id = await repository.CreateAsync(entry);
        logger.LogInformation("Body weight logged for user {UserId}: {Weight}kg", request.UserId, request.WeightKg);
        return id;
    }
}