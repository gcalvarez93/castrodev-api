// Path: src/Modules/Finance/Application/UseCases/GetBalance/GetBalanceHandler.cs
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetBalance;

public sealed class GetBalanceHandler(
    ITransactionRepository repository
) : IRequestHandler<GetBalanceQuery, decimal>
{
    public async Task<decimal> Handle(
        GetBalanceQuery request,
        CancellationToken cancellationToken)
    {
        return await repository.GetBalanceAsync(request.UserId);
    }
}