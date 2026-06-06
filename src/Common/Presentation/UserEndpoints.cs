// Path: src/Common/Presentation/UserEndpoints.cs
using Api.Common.Application.DTOs;
using Api.Common.Application.UseCases.GetUser;
using Api.Common.Application.UseCases.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Presentation;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/common/users")
            .RequireAuthorization();

        group.MapGet("/me", async (
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetUserQuery(userId));
            if (result is null) return Results.NotFound();

            return Results.Ok(result);
        });

        group.MapPut("/me", async (
            [FromBody] UpdateUserDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new UpdateUserCommand(userId, dto));
            return Results.Ok(result);
        });
    }
}