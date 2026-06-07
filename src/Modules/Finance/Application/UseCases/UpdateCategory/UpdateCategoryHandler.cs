// Path: src/Modules/Finance/Application/UseCases/UpdateCategory/UpdateCategoryHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.UpdateCategory;

public sealed class UpdateCategoryHandler(
    ICategoryRepository repository
) : IRequestHandler<UpdateCategoryCommand, CategoryResponseDto>
{
    public async Task<CategoryResponseDto> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) throw new KeyNotFoundException();

        var category = new Category
        {
            Id = request.Id,
            UserId = request.UserId,
            Name = request.Dto.Name,
            Color = request.Dto.Color,
            Icon = request.Dto.Icon,
            CreatedAt = existing.CreatedAt
        };

        await repository.UpdateAsync(category);

        return new CategoryResponseDto(
            category.Id,
            category.Name,
            category.Color,
            category.Icon,
            category.CreatedAt
        );
    }
}