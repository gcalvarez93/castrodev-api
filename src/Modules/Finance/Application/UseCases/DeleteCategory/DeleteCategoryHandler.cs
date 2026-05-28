// Path: src/Modules/Finance/Application/UseCases/DeleteCategory/DeleteCategoryHandler.cs
using Api.Modules.Finance.Domain.Repositories;
using MediatR;

namespace Api.Modules.Finance.Application.UseCases.DeleteCategory;

public sealed class DeleteCategoryHandler(
    ICategoryRepository repository
) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, request.UserId);
    }
}