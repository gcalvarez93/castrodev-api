// Path: src/Modules/WorkoutTracker/Presentation/WorkoutEndpoints.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Modules.WorkoutTracker.Application.DTOs;
using Api.Modules.WorkoutTracker.Application.UseCases.GetRoutines;
using Api.Modules.WorkoutTracker.Application.UseCases.GetRoutineById;
using Api.Modules.WorkoutTracker.Application.UseCases.CreateRoutine;
using Api.Modules.WorkoutTracker.Application.UseCases.UpdateRoutine;
using Api.Modules.WorkoutTracker.Application.UseCases.DeleteRoutine;
using Api.Modules.WorkoutTracker.Application.UseCases.GetExercises;
using Api.Modules.WorkoutTracker.Application.UseCases.AddExercise;
using Api.Modules.WorkoutTracker.Application.UseCases.UpdateExercise;
using Api.Modules.WorkoutTracker.Application.UseCases.DeleteExercise;
using Api.Modules.WorkoutTracker.Application.UseCases.StartSession;
using Api.Modules.WorkoutTracker.Application.UseCases.CompleteSession;
using Api.Modules.WorkoutTracker.Application.UseCases.GetSessions;
using Api.Modules.WorkoutTracker.Application.UseCases.GetSessionById;
using Api.Modules.WorkoutTracker.Application.UseCases.GetExerciseProgress;
using Api.Modules.WorkoutTracker.Application.UseCases.GetWeeklySummary;
using Api.Modules.WorkoutTracker.Application.UseCases.SearchExternalExercises;
using Api.Modules.WorkoutTracker.Application.UseCases.ImportExternalExercise;
using Api.Modules.WorkoutTracker.Application.UseCases.LogBodyWeight;
using Api.Modules.WorkoutTracker.Application.UseCases.GetBodyWeightHistory;

namespace Api.Modules.WorkoutTracker.Presentation;

public static class WorkoutEndpoints
{
    public static void MapWorkoutEndpoints(this WebApplication app)
    {
        var routines  = app.MapGroup("/api/workout/routines").RequireAuthorization();
        var sessions  = app.MapGroup("/api/workout/sessions").RequireAuthorization();
        var analytics = app.MapGroup("/api/workout/analytics").RequireAuthorization();
        var external  = app.MapGroup("/api/workout/external").RequireAuthorization();
        var body      = app.MapGroup("/api/workout/bodyweight").RequireAuthorization();

        // ── ROUTINES ──────────────────────────────────────────────────────────
        routines.MapGet("/", async (HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetRoutinesQuery(userId));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        routines.MapGet("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetRoutineByIdQuery(id, userId));
            return result.Match(Results.Ok, _ => Results.NotFound());
        });

        routines.MapPost("/", async (CreateRoutineRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new CreateRoutineCommand(
                userId, req.Name, req.Description, req.Category, req.DaysOfWeek,
                req.Exercises.Select(e => new ExerciseDto(e.Id, e.Name, e.MuscleGroup, e.Equipment,
                    e.Sets, e.Reps, e.WeightKg, e.RestSeconds, e.Notes, e.ExternalId, e.IsImported)).ToList()));
            return result.Match(id => Results.Created($"/api/workout/routines/{id}", id), _ => Results.BadRequest());
        });

        routines.MapPut("/{id}", async (string id, UpdateRoutineRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new UpdateRoutineCommand(
                id, userId, req.Name, req.Description, req.Category, req.DaysOfWeek,
                req.Exercises.Select(e => new ExerciseDto(e.Id, e.Name, e.MuscleGroup, e.Equipment,
                    e.Sets, e.Reps, e.WeightKg, e.RestSeconds, e.Notes, e.ExternalId, e.IsImported)).ToList()));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        routines.MapDelete("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new DeleteRoutineCommand(id, userId));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── EXERCISES ─────────────────────────────────────────────────────────
        routines.MapGet("/{routineId}/exercises", async (string routineId, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetExercisesQuery(userId, routineId));
            return result.Match(Results.Ok, _ => Results.NotFound());
        });

        routines.MapPost("/{routineId}/exercises", async (string routineId, ExerciseRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new AddExerciseCommand(userId, routineId,
                new ExerciseDto(req.Id, req.Name, req.MuscleGroup, req.Equipment,
                    req.Sets, req.Reps, req.WeightKg, req.RestSeconds, req.Notes, req.ExternalId, req.IsImported)));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        routines.MapPut("/{routineId}/exercises/{exerciseId}", async (string routineId, string exerciseId, ExerciseRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new UpdateExerciseCommand(userId, routineId, exerciseId,
                new ExerciseDto(exerciseId, req.Name, req.MuscleGroup, req.Equipment,
                    req.Sets, req.Reps, req.WeightKg, req.RestSeconds, req.Notes, req.ExternalId, req.IsImported)));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        routines.MapDelete("/{routineId}/exercises/{exerciseId}", async (string routineId, string exerciseId, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new DeleteExerciseCommand(userId, routineId, exerciseId));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── SESSIONS ──────────────────────────────────────────────────────────
        sessions.MapGet("/", async (HttpContext ctx, ISender sender, [FromQuery] string? routineId) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetSessionsQuery(userId, routineId));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        sessions.MapGet("/{id}", async (string id, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetSessionByIdQuery(id, userId));
            return result.Match(Results.Ok, _ => Results.NotFound());
        });

        sessions.MapPost("/start/{routineId}", async (string routineId, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new StartSessionCommand(userId, routineId));
            return result.Match(id => Results.Created($"/api/workout/sessions/{id}", id), _ => Results.NotFound());
        });

        sessions.MapPut("/{id}/complete", async (string id, CompleteSessionRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new CompleteSessionCommand(id, userId,
                req.Exercises.Select(e => new SessionExerciseDto(e.ExerciseId, e.Name,
                    e.Sets.Select(s => new SetLogDto(s.SetNumber, s.Reps, s.WeightKg, s.IsCompleted)).ToList(),
                    e.IsCompleted)).ToList()));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── ANALYTICS ─────────────────────────────────────────────────────────
        analytics.MapGet("/weekly/{year}/{week}", async (int year, int week, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetWeeklySummaryQuery(userId, year, week));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        analytics.MapGet("/progress/{exerciseName}", async (string exerciseName, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetExerciseProgressQuery(userId, exerciseName));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        // ── EXTERNAL EXERCISES (Wger) ─────────────────────────────────────────
        external.MapGet("/search", async (HttpContext ctx, ISender sender,
            [FromQuery] string? muscleGroup, [FromQuery] string? name) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new SearchExternalExercisesQuery(muscleGroup, name));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        external.MapPost("/import/{routineId}", async (string routineId, ImportExternalExerciseRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var dto = new ExternalExerciseDto(req.ExternalId, req.Name, req.MuscleGroup, req.Equipment, req.Difficulty, req.Instructions, req.GifUrl);
            var result = await sender.Send(new ImportExternalExerciseCommand(userId, routineId, dto, req.Sets, req.Reps, req.WeightKg, req.RestSeconds));
            return result.Match(_ => Results.NoContent(), _ => Results.NotFound());
        });

        // ── BODY WEIGHT ───────────────────────────────────────────────────────
        body.MapGet("/", async (HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new GetBodyWeightHistoryQuery(userId));
            return result.Match(Results.Ok, _ => Results.StatusCode(500));
        });

        body.MapPost("/", async (LogBodyWeightRequest req, HttpContext ctx, ISender sender) =>
        {
            var userId = ctx.User.FindFirst("user_id")?.Value;
            if (userId is null) return Results.Unauthorized();
            var result = await sender.Send(new LogBodyWeightCommand(userId, req.WeightKg, req.Date, req.Notes));
            return result.Match(id => Results.Created($"/api/workout/bodyweight/{id}", id), _ => Results.BadRequest());
        });
    }
}