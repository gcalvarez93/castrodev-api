// Path: src/Modules/BudgetScanner/Application/UseCases/CreateBudget/CreateBudgetHandler.cs
using ErrorOr;
using MediatR;
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;

namespace Api.Modules.BudgetScanner.Application.UseCases.CreateBudget;

public sealed class CreateBudgetHandler(
    IScannerBudgetRepository repository,
    ILogger<CreateBudgetHandler> logger
) : IRequestHandler<CreateBudgetCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(
        CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        if (request.Limit <= 0)
            return Error.Validation("Budget.InvalidLimit", "Limit must be greater than zero.");

        if (request.AlertThreshold is < 1 or > 100)
            return Error.Validation("Budget.InvalidThreshold", "Alert threshold must be between 1 and 100.");

        var budget = new ScannerBudget
        {
            UserId         = request.UserId,
            Name           = request.Name,
            Category       = request.Category,
            Limit          = request.Limit,
            Color          = request.Color,
            Currency       = request.Currency,
            IsRecurring    = request.IsRecurring,
            AlertThreshold = request.AlertThreshold,
            CreatedAt      = DateTime.UtcNow
        };

        var id = await repository.CreateAsync(budget);
        logger.LogInformation("Budget {Id} created for user {UserId}", id, request.UserId);
        return id;
    }
}