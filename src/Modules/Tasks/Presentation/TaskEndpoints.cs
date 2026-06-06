// Path: src/Modules/Tasks/Presentation/TaskEndpoints.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Application.UseCases.CreateTask;
using Api.Modules.Tasks.Application.UseCases.DeleteTask;
using Api.Modules.Tasks.Application.UseCases.GetTasks;
using Api.Modules.Tasks.Application.UseCases.UpdateTask;
using Api.Modules.Tasks.Application.UseCases.CreateBoard;
using Api.Modules.Tasks.Application.UseCases.GetBoards;
using Api.Modules.Tasks.Application.UseCases.DeleteBoard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Tasks.Presentation;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        var tasksGroup = app.MapGroup("/api/tasks")
            .RequireAuthorization();

        var boardsGroup = app.MapGroup("/api/tasks/boards")
            .RequireAuthorization();

        // Tasks
        tasksGroup.MapGet("/", async (
            string? boardId,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetTasksQuery(userId, boardId));
            return Results.Ok(result);
        });

        tasksGroup.MapPost("/", async (
            [FromBody] CreateTaskDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateTaskCommand(userId, dto));
            return Results.Created($"/api/tasks/{result.Id}", result);
        });

        tasksGroup.MapPut("/{id}", async (
            string id,
            [FromBody] UpdateTaskDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new UpdateTaskCommand(userId, id, dto));
            return Results.Ok(result);
        });

        tasksGroup.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            await sender.Send(new DeleteTaskCommand(userId, id));
            return Results.NoContent();
        });

        // Boards
        boardsGroup.MapGet("/", async (
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetBoardsQuery(userId));
            return Results.Ok(result);
        });

        boardsGroup.MapPost("/", async (
            [FromBody] CreateBoardDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateBoardCommand(userId, dto));
            return Results.Created($"/api/tasks/boards/{result.Id}", result);
        });

        boardsGroup.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            await sender.Send(new DeleteBoardCommand(userId, id));
            return Results.NoContent();
        });
    }
}