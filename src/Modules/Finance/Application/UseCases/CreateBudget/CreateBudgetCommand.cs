// Path: src/Modules/Finance/Application/UseCases/CreateBudget/CreateBudgetCommand.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateBudget;

public sealed record CreateBudgetCommand(
    string UserId,
    CreateBudgetDto Dto
) : IRequest<BudgetResponseDto>;