// Path: src/Modules/BudgetScanner/Presentation/BudgetScannerEndpoints.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Modules.BudgetScanner.Application.UseCases.CreateBudget;
using Api.Modules.BudgetScanner.Application.UseCases.CreateTransaction;
using Api.Modules.BudgetScanner.Application.UseCases.DeleteBudget;
using Api.Modules.BudgetScanner.Application.UseCases.DeleteTransaction;
using Api.Modules.BudgetScanner.Application.UseCases.GetBudgetById;
using Api.Modules.BudgetScanner.Application.UseCases.GetBudgets;
using Api.Modules.BudgetScanner.Application.UseCases.GetCategoryBreakdown;
using Api.Modules.BudgetScanner.Application.UseCases.GetMonthComparison;
using Api.Modules.BudgetScanner.Application.UseCases.GetMonthlySummary;
using Api.Modules.BudgetScanner.Application.UseCases.GetTransactionById;
using Api.Modules.BudgetScanner.Application.UseCases.GetTransactions;
using Api.Modules.BudgetScanner.Application.UseCases.ResetRecurringBudgets;
using Api.Modules.BudgetScanner.Application.UseCases.ScanReceipt;
using Api.Modules.BudgetScanner.Application.UseCases.UpdateBudget;
using Api.Modules.BudgetScanner.Application.UseCases.UpdateTransaction;

namespace Api.Modules.BudgetScanner.Presentation;

public static class BudgetScannerEndpoints
{
    public static void MapBudgetScannerEndpoints(this WebApplication app)
    {
        var budgets      = app.MapGroup("/api/budgetscanner/budgets").RequireAuthorization();
        var transactions = app.MapGroup("/api/budgetscanner/transactions").RequireAuthorization();
        var analytics    = app.MapGroup("/api/budgetscanner/analytics").RequireAuthorization();
        var receipts     = app.MapGroup("/api/budgetscanner/receipts").RequireAuthorization();

        // ── BUDGETS ──────────────────────────────────────────────────────────

        budgets.MapGet("/", async (HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetBudgetsQuery(userId));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        budgets.MapGet("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetBudgetByIdQuery(id, userId));
            return result.Match(Results.Ok, _ => Results.NotFound());
        });

        budgets.MapPost("/", async (CreateBudgetRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateBudgetCommand(
                userId, req.Name, req.Category, req.Limit,
                req.Color, req.Currency, req.IsRecurring, req.AlertThreshold));
            return result.Match(
                id => Results.Created($"/api/budgetscanner/budgets/{id}", id),
                _ => Results.BadRequest());
        });

        budgets.MapPut("/{id}", async (string id, UpdateBudgetRequest req,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new UpdateBudgetCommand(
                id, userId, req.Name, req.Category, req.Limit,
                req.Color, req.Currency, req.IsRecurring, req.AlertThreshold));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        budgets.MapDelete("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new DeleteBudgetCommand(id, userId));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        budgets.MapPost("/reset-recurring", async (HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new ResetRecurringBudgetsCommand(userId));
            return result.Match(_ => Results.NoContent(), _ => Results.StatusCode(500));
        });

        // ── TRANSACTIONS ─────────────────────────────────────────────────────

        transactions.MapGet("/", async (HttpContext ctx, ISender sender,
            [FromQuery] string? budgetId) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetTransactionsQuery(userId, budgetId));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        transactions.MapGet("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetTransactionByIdQuery(id, userId));
            return result.Match(Results.Ok, _ => Results.NotFound());
        });

        transactions.MapPost("/", async (CreateTransactionRequest req,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateTransactionCommand(
                userId, req.BudgetId, req.Category, req.Amount, req.Currency,
                req.Description, req.Notes, req.Tags,
                req.ReceiptImageUrl, req.Merchant, req.Date, req.IsScanned));
            return result.Match(
                id => Results.Created($"/api/budgetscanner/transactions/{id}", id),
                _ => Results.BadRequest());
        });

        transactions.MapPut("/{id}", async (string id, UpdateTransactionRequest req,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new UpdateTransactionCommand(
                id, userId, req.BudgetId, req.Category, req.Amount, req.Currency,
                req.Description, req.Notes, req.Tags,
                req.ReceiptImageUrl, req.Merchant, req.Date));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        transactions.MapDelete("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new DeleteTransactionCommand(id, userId));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── RECEIPTS (OCR) ───────────────────────────────────────────────────

        receipts.MapPost("/scan", async (ScanReceiptRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new ScanReceiptCommand(userId, req.ImageBase64));
            return result.Match(Results.Ok, _ => Results.BadRequest());
        });

        // ── ANALYTICS ────────────────────────────────────────────────────────

        analytics.MapGet("/summary/{year}/{month}", async (int year, int month,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetMonthlySummaryQuery(userId, year, month));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        analytics.MapGet("/categories/{year}/{month}", async (int year, int month,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetCategoryBreakdownQuery(userId, year, month));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        analytics.MapGet("/comparison/{year}/{month}", async (int year, int month,
            HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetMonthComparisonQuery(userId, year, month));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });
    }
}