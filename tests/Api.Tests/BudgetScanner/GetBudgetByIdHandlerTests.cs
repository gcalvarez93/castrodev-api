// Path: tests/Api.Tests/BudgetScanner/GetBudgetByIdHandlerTests.cs
using Api.Modules.BudgetScanner.Application.UseCases.GetBudgetById;
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace Api.Tests.BudgetScanner;

public sealed class GetBudgetByIdHandlerTests
{
    private const string UserId = "user-1";
    private const string BudgetId = "budget-1";

    private readonly Mock<IScannerBudgetRepository> _budgets = new();
    private readonly Mock<IScannerTransactionRepository> _transactions = new();

    private GetBudgetByIdHandler CreateHandler() => new(_budgets.Object, _transactions.Object);

    private void GivenBudgetWithSpent(decimal limit, decimal spent, int alertThreshold = 80)
    {
        _budgets
            .Setup(r => r.GetByIdAsync(BudgetId, UserId))
            .ReturnsAsync(new ScannerBudget
            {
                Id = BudgetId,
                UserId = UserId,
                Name = "Comida",
                Limit = limit,
                AlertThreshold = alertThreshold
            });
        _transactions
            .Setup(r => r.GetSpentByBudgetAsync(BudgetId, UserId, It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(spent);
    }

    [Fact]
    public async Task Handle_returns_not_found_when_the_budget_does_not_exist()
    {
        _budgets.Setup(r => r.GetByIdAsync(BudgetId, UserId)).ReturnsAsync((ScannerBudget?)null);

        var result = await CreateHandler().Handle(
            new GetBudgetByIdQuery(BudgetId, UserId), CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task Handle_computes_remaining_amount_and_percentage_below_the_threshold()
    {
        GivenBudgetWithSpent(limit: 200m, spent: 150m);

        var result = await CreateHandler().Handle(
            new GetBudgetByIdQuery(BudgetId, UserId), CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Spent.Should().Be(150m);
        result.Value.RemainingAmount.Should().Be(50m);
        result.Value.SpentPercentage.Should().Be(75m);
        result.Value.IsAlertTriggered.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_triggers_the_alert_when_spending_reaches_the_threshold()
    {
        GivenBudgetWithSpent(limit: 200m, spent: 160m);

        var result = await CreateHandler().Handle(
            new GetBudgetByIdQuery(BudgetId, UserId), CancellationToken.None);

        result.Value.SpentPercentage.Should().Be(80m);
        result.Value.IsAlertTriggered.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_reports_zero_percent_and_no_alert_when_the_limit_is_zero()
    {
        GivenBudgetWithSpent(limit: 0m, spent: 10m);

        var result = await CreateHandler().Handle(
            new GetBudgetByIdQuery(BudgetId, UserId), CancellationToken.None);

        result.Value.SpentPercentage.Should().Be(0m);
        result.Value.IsAlertTriggered.Should().BeFalse();
    }
}