// Path: src/Modules/Finance/Application/UseCases/UpdateCategory/UpdateCategoryCommand.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.UpdateCategory;

public sealed record UpdateCategoryCommand(
    string UserId,
    string Id,
    CreateCategoryDto Dto
) : IRequest<CategoryResponseDto>;