// Path: src/Modules/Tasks/Application/UseCases/GetBoards/GetBoardsQuery.cs
using Api.Modules.Tasks.Application.DTOs;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.GetBoards;

public sealed record GetBoardsQuery(string UserId) : IRequest<IEnumerable<BoardResponseDto>>;