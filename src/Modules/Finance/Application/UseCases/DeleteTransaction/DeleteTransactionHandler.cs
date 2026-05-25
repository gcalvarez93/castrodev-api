// Path: src/Modules/Finance/Application/UseCases/DeleteTransaction/DeleteTransactionHandler.cs
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.DeleteTransaction;

public sealed class DeleteTransactionHandler(
    ITransactionRepository repository
) : IRequestHandler<DeleteTransactionCommand>
{
    public async Task Handle(
        DeleteTransactionCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, request.UserId);
    }
}