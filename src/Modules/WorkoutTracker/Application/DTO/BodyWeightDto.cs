// Path: src/Modules/WorkoutTracker/Application/DTOs/BodyWeightDto.cs
namespace Api.Modules.WorkoutTracker.Application.DTOs;

public sealed record BodyWeightDto(
    string Id,
    double WeightKg,
    DateTime Date,
    string? Notes,
    DateTime CreatedAt
);