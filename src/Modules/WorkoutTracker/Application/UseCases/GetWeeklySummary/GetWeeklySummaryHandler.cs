// Path: src/Modules/WorkoutTracker/Application/UseCases/GetWeeklySummary/GetWeeklySummaryHandler.cs
using ErrorOr; using MediatR;
using System.Globalization;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Domain.Repositories;
namespace Api.Modules.WorkoutTracker.Application.UseCases.GetWeeklySummary;
public sealed class GetWeeklySummaryHandler(IWorkoutSessionRepository repository)
    : IRequestHandler<GetWeeklySummaryQuery, ErrorOr<WeeklySummaryDto>>
{
    public async Task<ErrorOr<WeeklySummaryDto>> Handle(GetWeeklySummaryQuery request, CancellationToken ct)
    {
        var firstDay = ISOWeek.ToDateTime(request.Year, request.Week, DayOfWeek.Monday);
        var lastDay  = firstDay.AddDays(7);
        var sessions = (await repository.GetByDateRangeAsync(request.UserId, firstDay, lastDay))
            .Where(s => s.IsCompleted).ToList();

        // Calcular racha
        var allSessions = (await repository.GetAllAsync(request.UserId))
            .Where(s => s.IsCompleted).OrderByDescending(s => s.CompletedAt).ToList();
        var streak = 0;
        var today  = DateTime.UtcNow.Date;
        var checkDate = today;
        foreach (var s in allSessions)
        {
            if (s.CompletedAt!.Value.Date == checkDate) { streak++; checkDate = checkDate.AddDays(-1); }
            else if (s.CompletedAt.Value.Date < checkDate) break;
        }

        return new WeeklySummaryDto(
            request.Year, request.Week,
            sessions.Select(s => s.CompletedAt!.Value.Date).Distinct().Count(),
            sessions.Sum(s => s.Exercises.Count(e => e.IsCompleted)),
            sessions.Sum(s => s.TotalVolumeKg),
            sessions.Sum(s => s.DurationMinutes),
            streak,
            sessions.Select(s => new SessionSummaryDto(
                s.Id, s.RoutineName, s.CompletedAt!.Value,
                s.DurationMinutes, s.TotalVolumeKg)).ToList()
        );
    }
}