// Path: src/Modules/Finance/Presentation/CategoryEndpoints.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Application.UseCases.CreateCategory;
using Api.Modules.Finance.Application.UseCases.DeleteCategory;
using Api.Modules.Finance.Application.UseCases.GetCategories;
using Api.Modules.Finance.Application.UseCases.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Finance.Presentation;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/finance/categories")
            .RequireAuthorization();

        group.MapPost("/", async (
            [FromBody] CreateCategoryDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateCategoryCommand(userId, dto));
            return Results.Created($"/api/finance/categories/{result.Id}", result);
        });

        group.MapGet("/", async (
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetCategoriesQuery(userId));
            return Results.Ok(result);
        });

        group.MapPut("/{id}", async (
            string id,
            [FromBody] CreateCategoryDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new UpdateCategoryCommand(userId, id, dto));
            return Results.Ok(result);
        });

        group.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            await sender.Send(new DeleteCategoryCommand(userId, id));
            return Results.NoContent();
        });
    }
}