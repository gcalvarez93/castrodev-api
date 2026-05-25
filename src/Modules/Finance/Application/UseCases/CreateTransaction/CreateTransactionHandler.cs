// Path: src/Modules/Finance/Application/UseCases/CreateTransaction/CreateTransactionHandler.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateTransaction;

public sealed class CreateTransactionHandler(
    ITransactionRepository repository
) : IRequestHandler<CreateTransactionCommand, TransactionResponseDto>
{
    public async Task<TransactionResponseDto> Handle(
        CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var transaction = new Transaction
        {
            UserId = request.UserId,
            Amount = request.Dto.Amount,
            Type = request.Dto.Type,
            CategoryId = request.Dto.CategoryId,
            Description = request.Dto.Description,
            Date = request.Dto.Date
        };

        var id = await repository.CreateAsync(transaction);

        return new TransactionResponseDto(
            id,
            transaction.Amount,
            transaction.Type,
            transaction.CategoryId,
            transaction.Description,
            transaction.Date,
            transaction.CreatedAt
        );
    }
}