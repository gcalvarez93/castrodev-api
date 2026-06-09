// Path: src/Modules/Habits/Presentation/HabitEndpoints.cs
using Api.Modules.Habits.Application.DTOs;
using Api.Modules.Habits.Application.UseCases.CompleteHabit;
using Api.Modules.Habits.Application.UseCases.CreateHabit;
using Api.Modules.Habits.Application.UseCases.DeleteHabit;
using Api.Modules.Habits.Application.UseCases.GetHabits;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Habits.Presentation;

public static class HabitEndpoints
{
    public static void MapHabitEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/habits")
            .RequireAuthorization();

        group.MapGet("/", async (
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetHabitsQuery(userId));
            return Results.Ok(result);
        });

        group.MapPost("/", async (
            [FromBody] CreateHabitDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateHabitCommand(userId, dto));
            return Results.Created($"/api/habits/{result.Id}", result);
        });

        group.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            await sender.Send(new DeleteHabitCommand(userId, id));
            return Results.NoContent();
        });

        group.MapPost("/{id}/complete", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CompleteHabitCommand(userId, id));
            return Results.Ok(result);
        });
    }
}