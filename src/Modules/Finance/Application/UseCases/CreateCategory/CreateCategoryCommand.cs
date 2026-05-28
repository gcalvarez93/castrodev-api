// Path: src/Modules/Finance/Application/UseCases/CreateCategory/CreateCategoryCommand.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateCategory;

public sealed record CreateCategoryCommand(
    string UserId,
    CreateCategoryDto Dto
) : IRequest<CategoryResponseDto>;