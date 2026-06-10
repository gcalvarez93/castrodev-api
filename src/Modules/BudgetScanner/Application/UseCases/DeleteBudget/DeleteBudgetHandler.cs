// Path: src/Modules/BudgetScanner/Application/Budgets/DeleteBudget/DeleteBudgetHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.DeleteBudget;

public sealed class DeleteBudgetHandler(
    IScannerBudgetRepository repository
) : IRequestHandler<DeleteBudgetCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, request.UserId);
        if (existing is null) return Error.NotFound("Budget.NotFound", "Budget not found.");

        await repository.DeleteAsync(request.Id, request.UserId);
        return Result.Deleted;
    }
}