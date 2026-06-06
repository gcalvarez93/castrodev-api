// Path: src/Common/Application/UseCases/UpdateUser/UpdateUserCommand.cs
using Api.Common.Application.DTOs;
using MediatR;

namespace Api.Common.Application.UseCases.UpdateUser;

public sealed record UpdateUserCommand(
    string UserId,
    UpdateUserDto Dto
) : IRequest<UserResponseDto>;