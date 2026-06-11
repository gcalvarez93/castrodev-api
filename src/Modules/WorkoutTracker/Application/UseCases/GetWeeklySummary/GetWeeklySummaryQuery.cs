// Path: src/Modules/WorkoutTracker/Application/UseCases/GetWeeklySummary/GetWeeklySummaryQuery.cs
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetWeeklySummary;
public sealed record GetWeeklySummaryQuery(string UserId, int Year, int Week) : IRequest<ErrorOr<WeeklySummaryDto>>;