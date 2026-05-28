// Path: src/Modules/Finance/Application/UseCases/DeleteBudget/DeleteBudgetHandler.cs
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.DeleteBudget;

public sealed class DeleteBudgetHandler(
    IBudgetRepository repository
) : IRequestHandler<DeleteBudgetCommand>
{
    public async Task Handle(
        DeleteBudgetCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, request.UserId);
    }
}