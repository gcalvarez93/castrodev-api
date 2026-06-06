// Path: src/Modules/Tasks/Application/UseCases/CreateBoard/CreateBoardCommand.cs
using Api.Modules.Tasks.Application.DTOs;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.CreateBoard;

public sealed record CreateBoardCommand(
    string UserId,
    CreateBoardDto Dto
) : IRequest<BoardResponseDto>;