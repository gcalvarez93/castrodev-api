// Path: src/Common/Application/UseCases/UpdateUser/UpdateUserHandler.cs
using Api.Common.Application.DTOs;
using Api.Common.Domain.Entities;
using Api.Common.Domain.Repositories;
using MediatR;

namespace Api.Common.Application.UseCases.UpdateUser;

public sealed class UpdateUserHandler(
    IUserRepository repository
) : IRequestHandler<UpdateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.UserId);

        var user = new User
        {
            Id = request.UserId,
            Name = request.Dto.Name,
            Email = existing?.Email ?? string.Empty,
            PhotoUrl = request.Dto.PhotoUrl,
            Language = request.Dto.Language,
            Notifications = request.Dto.Notifications,
            NotificationsGeneral = request.Dto.NotificationsGeneral,
            NotificationsTransactions = request.Dto.NotificationsTransactions,
            NotificationsBudgets = request.Dto.NotificationsBudgets,
            NotificationsReports = request.Dto.NotificationsReports,
            CreatedAt = existing?.CreatedAt ?? DateTime.UtcNow
        };

        if (existing is null)
            await repository.CreateAsync(user);
        else
            await repository.UpdateAsync(user);

        return new UserResponseDto(
            user.Id,
            user.Name,
            user.Email,
            user.PhotoUrl,
            user.Language,
            user.Notifications,
            user.NotificationsGeneral,
            user.NotificationsTransactions,
            user.NotificationsBudgets,
            user.NotificationsReports,
            user.CreatedAt
        );
    }
}