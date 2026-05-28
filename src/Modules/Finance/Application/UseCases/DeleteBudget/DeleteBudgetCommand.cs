// Path: src/Modules/Finance/Application/UseCases/DeleteBudget/DeleteBudgetCommand.cs
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.DeleteBudget;

public sealed record DeleteBudgetCommand(string UserId, string Id) : IRequest;