// Path: src/Modules/Tasks/Application/UseCases/GetBoards/GetBoardsHandler.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.GetBoards;

public sealed class GetBoardsHandler(
    IBoardRepository repository
) : IRequestHandler<GetBoardsQuery, IEnumerable<BoardResponseDto>>
{
    public async Task<IEnumerable<BoardResponseDto>> Handle(
        GetBoardsQuery request,
        CancellationToken cancellationToken)
    {
        var boards = await repository.GetAllAsync(request.UserId);

        return boards.Select(b => new BoardResponseDto(
            b.Id,
            b.Name,
            b.Description,
            b.Color,
            b.TaskCount,
            b.CreatedAt
        ));
    }
}