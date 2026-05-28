// Path: src/Modules/Finance/Application/UseCases/GetCategories/GetCategoriesQuery.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetCategories;

public sealed record GetCategoriesQuery(string UserId) : IRequest<IEnumerable<CategoryResponseDto>>;