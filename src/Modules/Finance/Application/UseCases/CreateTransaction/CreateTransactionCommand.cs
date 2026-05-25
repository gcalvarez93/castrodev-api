// Path: src/Modules/Finance/Application/UseCases/CreateTransaction/CreateTransactionCommand.cs
using Api.Modules.Finance.Application.DTOs;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.CreateTransaction;

public sealed record CreateTransactionCommand(
    string UserId,
    CreateTransactionDto Dto
) : IRequest<TransactionResponseDto>;