// Path: src/Modules/Tasks/Domain/Repositories/ILabelRepository.cs
using Api.Modules.Tasks.Domain.Entities;

namespace Api.Modules.Tasks.Domain.Repositories;

public interface ILabelRepository
{
    Task<IEnumerable<Label>> GetAllAsync(string userId);
    Task<string> CreateAsync(Label label);
    Task DeleteAsync(string id, string userId);
}