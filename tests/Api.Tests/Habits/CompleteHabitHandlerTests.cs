// Path: tests/Api.Tests/Habits/CompleteHabitHandlerTests.cs
using Api.Modules.Habits.Application.UseCases.CompleteHabit;
using Api.Modules.Habits.Domain.Entities;
using Api.Modules.Habits.Domain.Repositories;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace Api.Tests.Habits;

public sealed class CompleteHabitHandlerTests
{
    private const string UserId = "user-1";
    private const string HabitId = "habit-1";

    private readonly Mock<IHabitRepository> _habits = new();
    private readonly Mock<IHabitCompletionRepository> _completions = new();

    private CompleteHabitHandler CreateHandler() => new(_habits.Object, _completions.Object);

    private static Habit CreateHabit(int streak, int bestStreak, int totalCompleted) => new()
    {
        Id = HabitId,
        UserId = UserId,
        Name = "Leer",
        Streak = streak,
        BestStreak = bestStreak,
        TotalCompleted = totalCompleted
    };

    [Fact]
    public async Task Handle_returns_not_found_when_the_habit_does_not_exist()
    {
        _habits.Setup(r => r.GetByIdAsync(HabitId, UserId)).ReturnsAsync((Habit?)null);

        var result = await CreateHandler().Handle(
            new CompleteHabitCommand(UserId, HabitId), CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _completions.Verify(r => r.CreateAsync(It.IsAny<HabitCompletion>()), Times.Never);
        _habits.Verify(r => r.UpdateAsync(It.IsAny<Habit>()), Times.Never);
    }

    [Fact]
    public async Task Handle_increments_streak_and_total_and_records_the_completion()
    {
        _habits.Setup(r => r.GetByIdAsync(HabitId, UserId)).ReturnsAsync(CreateHabit(3, 5, 10));

        var result = await CreateHandler().Handle(
            new CompleteHabitCommand(UserId, HabitId), CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Streak.Should().Be(4);
        result.Value.BestStreak.Should().Be(5);
        result.Value.TotalCompleted.Should().Be(11);
        result.Value.CompletedToday.Should().BeTrue();
        _completions.Verify(
            r => r.CreateAsync(It.Is<HabitCompletion>(c => c.HabitId == HabitId && c.UserId == UserId)),
            Times.Once);
        _habits.Verify(
            r => r.UpdateAsync(It.Is<Habit>(h => h.Id == HabitId && h.Streak == 4 && h.TotalCompleted == 11)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_raises_the_best_streak_when_the_current_one_beats_it()
    {
        _habits.Setup(r => r.GetByIdAsync(HabitId, UserId)).ReturnsAsync(CreateHabit(5, 5, 20));

        var result = await CreateHandler().Handle(
            new CompleteHabitCommand(UserId, HabitId), CancellationToken.None);

        result.Value.Streak.Should().Be(6);
        result.Value.BestStreak.Should().Be(6);
    }
}