// Path: src/Modules/Finance/Presentation/BudgetEndpoints.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Application.UseCases.CreateBudget;
using Api.Modules.Finance.Application.UseCases.DeleteBudget;
using Api.Modules.Finance.Application.UseCases.GetBudgets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Finance.Presentation;

public static class BudgetEndpoints
{
    public static void MapBudgetEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/finance/budgets")
            .RequireAuthorization();

        group.MapPost("/", async (
            [FromBody] CreateBudgetDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateBudgetCommand(userId, dto));
            return Results.Created($"/api/finance/budgets/{result.Id}", result);
        });

        group.MapGet("/", async (
            string month,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetBudgetsQuery(userId, month));
            return Results.Ok(result);
        });

        group.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            await sender.Send(new DeleteBudgetCommand(userId, id));
            return Results.NoContent();
        });
    }
}