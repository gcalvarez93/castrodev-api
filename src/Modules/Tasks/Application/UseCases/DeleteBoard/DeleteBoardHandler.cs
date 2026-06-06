// Path: src/Modules/Tasks/Application/UseCases/DeleteBoard/DeleteBoardHandler.cs
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.DeleteBoard;

public sealed class DeleteBoardHandler(
    IBoardRepository repository
) : IRequestHandler<DeleteBoardCommand>
{
    public async Task Handle(
        DeleteBoardCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, request.UserId);
    }
}