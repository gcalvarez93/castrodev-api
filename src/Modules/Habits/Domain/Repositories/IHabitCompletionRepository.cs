// Path: src/Modules/Habits/Domain/Repositories/IHabitCompletionRepository.cs
using Api.Modules.Habits.Domain.Entities;

namespace Api.Modules.Habits.Domain.Repositories;

public interface IHabitCompletionRepository
{
    Task<IEnumerable<HabitCompletion>> GetByHabitIdAsync(string habitId, string userId);
    Task<IEnumerable<HabitCompletion>> GetByDateAsync(string userId, DateTime date);
    Task<string> CreateAsync(HabitCompletion completion);
    Task DeleteAsync(string id, string userId);
}