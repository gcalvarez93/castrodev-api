// Path: src/Modules/Tasks/Application/UseCases/CreateBoard/CreateBoardHandler.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.CreateBoard;

public sealed class CreateBoardHandler(
    IBoardRepository repository
) : IRequestHandler<CreateBoardCommand, BoardResponseDto>
{
    public async Task<BoardResponseDto> Handle(
        CreateBoardCommand request,
        CancellationToken cancellationToken)
    {
        var board = new Board
        {
            UserId = request.UserId,
            Name = request.Dto.Name,
            Color = request.Dto.Color
        };

        var id = await repository.CreateAsync(board);

        return new BoardResponseDto(
            id,
            board.Name,
            board.Color,
            board.CreatedAt
        );
    }
}