// Path: src/Modules/Finance/Application/UseCases/GetBalance/GetBalanceQuery.cs
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.GetBalance;

public sealed record GetBalanceQuery(string UserId) : IRequest<decimal>;