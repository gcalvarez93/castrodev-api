// Path: src/Modules/WorkoutTracker/Domain/Repositories/IWorkoutSessionRepository.cs
using Api.Modules.WorkoutTracker.Domain.Entities;

namespace Api.Modules.WorkoutTracker.Domain.Repositories;

public interface IWorkoutSessionRepository
{
    Task<IEnumerable<WorkoutSession>> GetAllAsync(string userId, string? routineId = null);
    Task<WorkoutSession?> GetByIdAsync(string id, string userId);
    Task<string> CreateAsync(WorkoutSession session);
    Task UpdateAsync(WorkoutSession session);
    Task<IEnumerable<WorkoutSession>> GetByDateRangeAsync(string userId, DateTime from, DateTime to);
}