// Path: src/Common/Application/UseCases/GetUser/GetUserHandler.cs
using Api.Common.Application.DTOs;
using Api.Common.Domain.Repositories;
using MediatR;

namespace Api.Common.Application.UseCases.GetUser;

public sealed class GetUserHandler(
    IUserRepository repository
) : IRequestHandler<GetUserQuery, UserResponseDto?>
{
    public async Task<UserResponseDto?> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId);
        if (user is null) return null;

        return new UserResponseDto(
            user.Id,
            user.Name,
            user.Email,
            user.PhotoUrl,
            user.Language,
            user.Notifications,
            user.CreatedAt
        );
    }
}