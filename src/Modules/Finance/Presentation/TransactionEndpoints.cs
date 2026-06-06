// Path: src/Modules/Finance/Presentation/TransactionEndpoints.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Application.UseCases.CreateTransaction;
using Api.Modules.Finance.Application.UseCases.DeleteTransaction;
using Api.Modules.Finance.Application.UseCases.ExportTransactions;
using Api.Modules.Finance.Application.UseCases.GetBalance;
using Api.Modules.Finance.Application.UseCases.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Finance.Presentation;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/finance/transactions")
            .RequireAuthorization();

        group.MapPost("/", async (
            [FromBody] CreateTransactionDto dto,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new CreateTransactionCommand(userId, dto));
            return Results.Created($"/api/finance/transactions/{result.Id}", result);
        });

        group.MapGet("/", async (
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetTransactionsQuery(userId));
            return Results.Ok(result);
        });

        group.MapGet("/balance", async (
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var result = await sender.Send(new GetBalanceQuery(userId));
            return Results.Ok(result);
        });

        group.MapDelete("/{id}", async (
            string id,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            await sender.Send(new DeleteTransactionCommand(userId, id));
            return Results.NoContent();
        });

        group.MapGet("/export/pdf", async (
            string month,
            string userName,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var bytes = await sender.Send(new ExportTransactionsPdfQuery(userId, userName, month));
            return Results.File(bytes, "application/pdf", $"transacciones-{month}.pdf");
        });

        group.MapGet("/export/excel", async (
            string month,
            HttpContext context,
            ISender sender) =>
        {
            var userId = context.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();

            var bytes = await sender.Send(new ExportTransactionsExcelQuery(userId, month));
            return Results.File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"transacciones-{month}.xlsx");
        });
    }
}