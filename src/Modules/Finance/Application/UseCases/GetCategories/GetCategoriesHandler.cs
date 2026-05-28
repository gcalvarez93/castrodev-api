// Path: src/Modules/Finance/Application/UseCases/GetCategories/GetCategoriesHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetCategories;

public sealed class GetCategoriesHandler(
    ICategoryRepository repository
) : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryResponseDto>>
{
    public async Task<IEnumerable<CategoryResponseDto>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await repository.GetAllAsync(request.UserId);

        return categories.Select(c => new CategoryResponseDto(
            c.Id,
            c.Name,
            c.Color,
            c.Icon,
            c.CreatedAt
        ));
    }
}