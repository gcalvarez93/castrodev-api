// Path: tests/Api.Tests/Tasks/UpdateTaskHandlerTests.cs
using Api.Modules.Tasks.Application.DTOs;
using Api.Modules.Tasks.Application.UseCases.UpdateTask;
using Api.Modules.Tasks.Domain.Entities;
using Api.Modules.Tasks.Domain.Repositories;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace Api.Tests.Tasks;

public sealed class UpdateTaskHandlerTests
{
    private const string UserId = "user-1";
    private const string TaskId = "task-1";

    private readonly Mock<ITaskRepository> _tasks = new();

    private UpdateTaskHandler CreateHandler() => new(_tasks.Object);

    private static UpdateTaskCommand CreateCommand() => new(
        UserId,
        TaskId,
        new UpdateTaskDto("Nuevo título", "Detalle", "done", "high", null, new List<string> { "urgente" }));

    [Fact]
    public async Task Handle_returns_not_found_when_the_task_does_not_exist()
    {
        _tasks.Setup(r => r.GetByIdAsync(TaskId, UserId)).ReturnsAsync((TaskItem?)null);

        var result = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _tasks.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Never);
    }

    [Fact]
    public async Task Handle_updates_the_fields_and_keeps_board_and_creation_date()
    {
        var createdAt = new DateTime(2026, 2, 1, 8, 0, 0, DateTimeKind.Utc);
        _tasks
            .Setup(r => r.GetByIdAsync(TaskId, UserId))
            .ReturnsAsync(new TaskItem { Id = TaskId, UserId = UserId, BoardId = "board-9", CreatedAt = createdAt });

        var result = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Title.Should().Be("Nuevo título");
        result.Value.Status.Should().Be("done");
        result.Value.BoardId.Should().Be("board-9");
        result.Value.CreatedAt.Should().Be(createdAt);
        _tasks.Verify(
            r => r.UpdateAsync(It.Is<TaskItem>(t => t.Id == TaskId && t.BoardId == "board-9" && t.Priority == "high")),
            Times.Once);
    }
}