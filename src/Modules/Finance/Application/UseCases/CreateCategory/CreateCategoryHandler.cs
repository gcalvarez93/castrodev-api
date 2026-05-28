// Path: src/Modules/Finance/Application/UseCases/CreateCategory/CreateCategoryHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateCategory;

public sealed class CreateCategoryHandler(
    ICategoryRepository repository
) : IRequestHandler<CreateCategoryCommand, CategoryResponseDto>
{
    public async Task<CategoryResponseDto> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = new Category
        {
            UserId = request.UserId,
            Name = request.Dto.Name,
            Color = request.Dto.Color,
            Icon = request.Dto.Icon
        };

        var id = await repository.CreateAsync(category);

        return new CategoryResponseDto(
            id,
            category.Name,
            category.Color,
            category.Icon,
            category.CreatedAt
        );
    }
}