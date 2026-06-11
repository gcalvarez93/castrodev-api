// Path: src/Modules/WorkoutTracker/Domain/Repositories/IRoutineRepository.cs
using Api.Modules.WorkoutTracker.Domain.Entities;

namespace Api.Modules.WorkoutTracker.Domain.Repositories;

public interface IRoutineRepository
{
    Task<IEnumerable<Routine>> GetAllAsync(string userId);
    Task<Routine?> GetByIdAsync(string id, string userId);
    Task<string> CreateAsync(Routine routine);
    Task UpdateAsync(Routine routine);
    Task DeleteAsync(string id, string userId);
}