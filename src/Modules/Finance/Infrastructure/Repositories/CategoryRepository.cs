// Path: src/Modules/Finance/Infrastructure/Repositories/CategoryRepository.cs
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Finance.Infrastructure.Repositories;

public sealed class CategoryRepository(FirestoreDb db) : ICategoryRepository
{
    private CollectionReference Collection => db.Collection("categories_financetracker");

    public async Task<Category?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var category = doc.ConvertTo<CategoryDocument>();
        if (category.UserId != userId) return null;

        return MapToDomain(doc.Id, category);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderBy("name")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<CategoryDocument>()));
    }

    public async Task<string> CreateAsync(Category category)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new CategoryDocument
        {
            UserId = category.UserId,
            Name = category.Name,
            Color = category.Color,
            Icon = category.Icon,
            CreatedAt = category.CreatedAt
        });
        return doc.Id;
    }

    public async Task UpdateAsync(Category category)
    {
        await Collection.Document(category.Id).SetAsync(new CategoryDocument
        {
            UserId = category.UserId,
            Name = category.Name,
            Color = category.Color,
            Icon = category.Icon,
            CreatedAt = category.CreatedAt
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var category = doc.ConvertTo<CategoryDocument>();
        if (category.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static Category MapToDomain(string id, CategoryDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        Name = doc.Name,
        Color = doc.Color,
        Icon = doc.Icon,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class CategoryDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]      public string Name { get; set; } = string.Empty;
    [FirestoreProperty("color")]     public string Color { get; set; } = string.Empty;
    [FirestoreProperty("icon")]      public string Icon { get; set; } = string.Empty;
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}