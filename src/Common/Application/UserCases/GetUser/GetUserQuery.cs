// Path: src/Common/Application/UseCases/GetUser/GetUserQuery.cs
using Api.Common.Application.DTOs;
using MediatR;

namespace Api.Common.Application.UseCases.GetUser;

public sealed record GetUserQuery(string UserId) : IRequest<UserResponseDto?>;