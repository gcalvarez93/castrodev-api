// Path: src/Modules/Finance/Application/UseCases/DeleteTransaction/DeleteTransactionCommand.cs
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.DeleteTransaction;

public sealed record DeleteTransactionCommand(string UserId, string Id) : IRequest;