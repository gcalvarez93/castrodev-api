// Path: src/Modules/Habits/Application/UseCases/DeleteHabit/DeleteHabitHandler.cs
using Api.Modules.Habits.Domain.Repositories;
using MediatR;

namespace Api.Modules.Habits.Application.UseCases.DeleteHabit;

public sealed class DeleteHabitHandler(
    IHabitRepository repository
) : IRequestHandler<DeleteHabitCommand>
{
    public async Task Handle(
        DeleteHabitCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, request.UserId);
    }
}