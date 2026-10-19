// Path: tests/Api.Tests/Finance/UpdateCategoryHandlerTests.cs
using Api.Modules.Finance.Application.DTOs;
using Api.Modules.Finance.Application.UseCases.UpdateCategory;
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace Api.Tests.Finance;

public sealed class UpdateCategoryHandlerTests
{
    private const string UserId = "user-1";
    private const string CategoryId = "cat-1";

    private readonly Mock<ICategoryRepository> _categories = new();

    private UpdateCategoryHandler CreateHandler() => new(_categories.Object);

    private static UpdateCategoryCommand CreateCommand() =>
        new(UserId, CategoryId, new CreateCategoryDto("Comida", "#FF0000", "restaurant"));

    [Fact]
    public async Task Handle_returns_not_found_when_the_category_does_not_exist()
    {
        _categories.Setup(r => r.GetByIdAsync(CategoryId, UserId)).ReturnsAsync((Category?)null);

        var result = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.NotFound);
        _categories.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task Handle_updates_the_fields_and_keeps_the_creation_date()
    {
        var createdAt = new DateTime(2026, 1, 10, 9, 0, 0, DateTimeKind.Utc);
        _categories
            .Setup(r => r.GetByIdAsync(CategoryId, UserId))
            .ReturnsAsync(new Category { Id = CategoryId, UserId = UserId, Name = "Antes", CreatedAt = createdAt });

        var result = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be("Comida");
        result.Value.Color.Should().Be("#FF0000");
        result.Value.CreatedAt.Should().Be(createdAt);
        _categories.Verify(
            r => r.UpdateAsync(It.Is<Category>(c => c.Id == CategoryId && c.Name == "Comida" && c.CreatedAt == createdAt)),
            Times.Once);
    }
}