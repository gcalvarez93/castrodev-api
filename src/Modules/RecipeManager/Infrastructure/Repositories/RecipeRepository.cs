// Path: src/Modules/RecipeManager/Infrastructure/Repositories/RecipeRepository.cs
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.RecipeManager.Infrastructure.Repositories;

public sealed class RecipeRepository(FirestoreDb db) : IRecipeRepository
{
    private CollectionReference Collection => db.Collection("recipes_recipemanager");

    public async Task<IEnumerable<Recipe>> GetAllAsync(string userId, string? category = null, bool? favoritesOnly = null)
    {
        Query query = Collection.WhereEqualTo("userId", userId);
        if (!string.IsNullOrWhiteSpace(category))
            query = query.WhereEqualTo("category", category);
        if (favoritesOnly == true)
            query = query.WhereEqualTo("isFavorite", true);

        var snapshot = await query.OrderBy("name").GetSnapshotAsync();
        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<RecipeDocument>()));
    }

    public async Task<Recipe?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;
        var data = doc.ConvertTo<RecipeDocument>();
        if (data.UserId != userId) return null;
        return MapToDomain(doc.Id, data);
    }

    public async Task<string> CreateAsync(Recipe recipe)
    {
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(recipe));
        return doc.Id;
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        await Collection.Document(recipe.Id).SetAsync(ToDocument(recipe), SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;
        if (doc.ConvertTo<RecipeDocument>().UserId != userId) return;
        await Collection.Document(id).DeleteAsync();
    }

    public async Task ToggleFavoriteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;
        var data = doc.ConvertTo<RecipeDocument>();
        if (data.UserId != userId) return;
        await Collection.Document(id).UpdateAsync("isFavorite", !data.IsFavorite);
    }

    private static Recipe MapToDomain(string id, RecipeDocument doc) => new()
    {
        Id              = id,
        UserId          = doc.UserId,
        Name            = doc.Name,
        Description     = doc.Description,
        Category        = doc.Category,
        ImageUrl        = doc.ImageUrl,
        PrepTimeMinutes = doc.PrepTimeMinutes,
        CookTimeMinutes = doc.CookTimeMinutes,
        Servings        = doc.Servings,
        Difficulty      = doc.Difficulty,
        Ingredients     = doc.Ingredients?.Select(i => new Ingredient { Name = i.Name, Amount = i.Amount, Unit = i.Unit }).ToList() ?? [],
        Steps           = doc.Steps?.Select(s => new RecipeStep { Order = s.Order, Description = s.Description }).ToList() ?? [],
        Tags            = doc.Tags ?? [],
        IsFavorite      = doc.IsFavorite,
        ExternalId      = doc.ExternalId,
        IsImported      = doc.IsImported,
        CreatedAt       = doc.CreatedAt
    };

    private static RecipeDocument ToDocument(Recipe r) => new()
    {
        UserId          = r.UserId,
        Name            = r.Name,
        Description     = r.Description,
        Category        = r.Category,
        ImageUrl        = r.ImageUrl,
        PrepTimeMinutes = r.PrepTimeMinutes,
        CookTimeMinutes = r.CookTimeMinutes,
        Servings        = r.Servings,
        Difficulty      = r.Difficulty,
        Ingredients     = r.Ingredients.Select(i => new IngredientDocument { Name = i.Name, Amount = i.Amount, Unit = i.Unit }).ToList(),
        Steps           = r.Steps.Select(s => new RecipeStepDocument { Order = s.Order, Description = s.Description }).ToList(),
        Tags            = r.Tags,
        IsFavorite      = r.IsFavorite,
        ExternalId      = r.ExternalId,
        IsImported      = r.IsImported,
        CreatedAt       = r.CreatedAt
    };
}

[FirestoreData] internal sealed class RecipeDocument
{
    [FirestoreProperty("userId")]          public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]            public string Name { get; set; } = string.Empty;
    [FirestoreProperty("description")]     public string Description { get; set; } = string.Empty;
    [FirestoreProperty("category")]        public string Category { get; set; } = string.Empty;
    [FirestoreProperty("imageUrl")]        public string ImageUrl { get; set; } = string.Empty;
    [FirestoreProperty("prepTimeMinutes")] public int PrepTimeMinutes { get; set; }
    [FirestoreProperty("cookTimeMinutes")] public int CookTimeMinutes { get; set; }
    [FirestoreProperty("servings")]        public int Servings { get; set; }
    [FirestoreProperty("difficulty")]      public string Difficulty { get; set; } = "medium";
    [FirestoreProperty("ingredients")]     public List<IngredientDocument>? Ingredients { get; set; }
    [FirestoreProperty("steps")]           public List<RecipeStepDocument>? Steps { get; set; }
    [FirestoreProperty("tags")]            public List<string>? Tags { get; set; }
    [FirestoreProperty("isFavorite")]      public bool IsFavorite { get; set; }
    [FirestoreProperty("externalId")]      public string? ExternalId { get; set; }
    [FirestoreProperty("isImported")]      public bool IsImported { get; set; }
    [FirestoreProperty("createdAt")]       public DateTime CreatedAt { get; set; }
}

[FirestoreData] internal sealed class IngredientDocument
{
    [FirestoreProperty("name")]   public string Name { get; set; } = string.Empty;
    [FirestoreProperty("amount")] public string Amount { get; set; } = string.Empty;
    [FirestoreProperty("unit")]   public string Unit { get; set; } = string.Empty;
}

[FirestoreData] internal sealed class RecipeStepDocument
{
    [FirestoreProperty("order")]       public int Order { get; set; }
    [FirestoreProperty("description")] public string Description { get; set; } = string.Empty;
}