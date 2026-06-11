// Path: src/Modules/WorkoutTracker/Application/UseCases/LogBodyWeight/LogBodyWeightCommand.cs
using ErrorOr; using MediatR;
namespace Api.Modules.WorkoutTracker.Application.UseCases.LogBodyWeight;
public sealed record LogBodyWeightCommand(string UserId, double WeightKg, DateTime Date, string? Notes) : IRequest<ErrorOr<string>>;