// Path: src/Modules/Tasks/Application/UseCases/DeleteTask/DeleteTaskHandler.cs
using Api.Modules.Tasks.Domain.Repositories;
using MediatR;

namespace Api.Modules.Tasks.Application.UseCases.DeleteTask;

public sealed class DeleteTaskHandler(
    ITaskRepository repository
) : IRequestHandler<DeleteTaskCommand>
{
    public async Task Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, request.UserId);
    }
}