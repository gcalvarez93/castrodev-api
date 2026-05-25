// Path: src/Modules/Finance/Application/UseCases/GetTransactions/GetTransactionsHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetTransactions;

public sealed class GetTransactionsHandler(
    ITransactionRepository repository
) : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionResponseDto>>
{
    public async Task<IEnumerable<TransactionResponseDto>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await repository.GetAllAsync(request.UserId);

        return transactions.Select(t => new TransactionResponseDto(
            t.Id,
            t.Amount,
            t.Type,
            t.CategoryId,
            t.Description,
            t.Date,
            t.CreatedAt
        ));
    }
}