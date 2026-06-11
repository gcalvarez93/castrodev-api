// Path: src/Modules/RecipeManager/Presentation/RecipeEndpoints.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Modules.RecipeManager.Application.DTOs;
using Api.Modules.RecipeManager.Application.UseCases;

namespace Api.Modules.RecipeManager.Presentation;

public static class RecipeEndpoints
{
    public static void MapRecipeEndpoints(this WebApplication app)
    {
        var recipes   = app.MapGroup("/api/recipes").RequireAuthorization();
        var external  = app.MapGroup("/api/recipes/external").RequireAuthorization();
        var mealplan  = app.MapGroup("/api/recipes/mealplan").RequireAuthorization();
        var shopping  = app.MapGroup("/api/recipes/shopping").RequireAuthorization();

        // ── RECIPES ──────────────────────────────────────────────────────────

        recipes.MapGet("/", async (HttpContext ctx, ISender sender,
            [FromQuery] string? category, [FromQuery] bool? favoritesOnly) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetRecipesQuery(userId, category, favoritesOnly));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        recipes.MapGet("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetRecipeByIdQuery(id, userId));
            return result.Match(Results.Ok, _ => Results.NotFound());
        });

        recipes.MapPost("/", async (CreateRecipeRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new CreateRecipeCommand(
                userId, req.Name, req.Description, req.Category, req.ImageUrl,
                req.PrepTimeMinutes, req.CookTimeMinutes, req.Servings, req.Difficulty,
                req.Ingredients.Select(i => new IngredientDto(i.Name, i.Amount, i.Unit)).ToList(),
                req.Steps.Select(s => new RecipeStepDto(s.Order, s.Description)).ToList(),
                req.Tags));
            return result.Match(id => Results.Created($"/api/recipes/{id}", id), _ => Results.BadRequest());
        });

        recipes.MapPut("/{id}", async (string id, UpdateRecipeRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new UpdateRecipeCommand(
                id, userId, req.Name, req.Description, req.Category, req.ImageUrl,
                req.PrepTimeMinutes, req.CookTimeMinutes, req.Servings, req.Difficulty,
                req.Ingredients.Select(i => new IngredientDto(i.Name, i.Amount, i.Unit)).ToList(),
                req.Steps.Select(s => new RecipeStepDto(s.Order, s.Description)).ToList(),
                req.Tags));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        recipes.MapDelete("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new DeleteRecipeCommand(id, userId));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        recipes.MapPost("/{id}/favorite", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new ToggleFavoriteCommand(id, userId));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── EXTERNAL RECIPES (TheMealDB) ──────────────────────────────────────

        external.MapGet("/search", async (HttpContext ctx, ISender sender, [FromQuery] string q) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new SearchExternalRecipesQuery(q));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        external.MapPost("/import", async (ImportExternalRecipeRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var dto = new ExternalRecipeDto(
                req.ExternalId, req.Name, req.Category, req.Area, req.Instructions, req.ImageUrl,
                req.Ingredients.Select(i => new IngredientDto(i.Name, i.Amount, i.Unit)).ToList(),
                req.YoutubeUrl);
            var result = await sender.Send(new ImportExternalRecipeCommand(userId, dto));
            return result.Match(id => Results.Created($"/api/recipes/{id}", id), _ => Results.BadRequest());
        });

        // ── MEAL PLAN ─────────────────────────────────────────────────────────

        mealplan.MapGet("/{year}/{week}", async (int year, int week, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetMealPlanQuery(userId, year, week));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        mealplan.MapPut("/{year}/{week}", async (int year, int week, UpsertMealPlanEntryRequest req,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new UpsertMealPlanEntryCommand(
                userId, year, week, req.DayOfWeek, req.MealType, req.RecipeId, req.RecipeName));
            return result.Match(id => Results.Ok(id), _ => Results.BadRequest());
        });

        mealplan.MapDelete("/{year}/{week}/{dayOfWeek}/{mealType}", async (
            int year, int week, string dayOfWeek, string mealType, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new DeleteMealPlanEntryCommand(userId, year, week, dayOfWeek, mealType));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── SHOPPING LIST ─────────────────────────────────────────────────────

        shopping.MapGet("/{year}/{week}", async (int year, int week, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetShoppingListQuery(userId, year, week));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        shopping.MapPost("/{year}/{week}/generate", async (int year, int week, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GenerateShoppingListCommand(userId, year, week));
            return result.Match(id => Results.Ok(id), _ => Results.BadRequest());
        });

        shopping.MapPost("/{id}/toggle/{itemName}", async (string id, string itemName,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new ToggleShoppingItemCommand(id, userId, itemName));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });
    }
}