// Path: src/Modules/RecipeManager/Infrastructure/Repositories/MealPlanRepository.cs
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.RecipeManager.Infrastructure.Repositories;

public sealed class MealPlanRepository(FirestoreDb db) : IMealPlanRepository
{
    private CollectionReference Collection => db.Collection("mealplans_recipemanager");

    public async Task<MealPlan?> GetByWeekAsync(string userId, int year, int week)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("year", year)
            .WhereEqualTo("week", week)
            .Limit(1)
            .GetSnapshotAsync();

        if (snapshot.Count == 0) return null;
        var doc = snapshot.Documents[0];
        return MapToDomain(doc.Id, doc.ConvertTo<MealPlanDocument>());
    }

    public async Task<string> CreateOrUpdateAsync(MealPlan plan)
    {
        if (!string.IsNullOrEmpty(plan.Id))
        {
            await Collection.Document(plan.Id).SetAsync(ToDocument(plan), SetOptions.Overwrite);
            return plan.Id;
        }
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(plan));
        return doc.Id;
    }

    public async Task DeleteEntryAsync(string userId, int year, int week, string dayOfWeek, string mealType)
    {
        var plan = await GetByWeekAsync(userId, year, week);
        if (plan is null) return;

        var updatedEntries = plan.Entries
            .Where(e => !(e.DayOfWeek == dayOfWeek && e.MealType == mealType))
            .ToList();

        var updated = new MealPlan
        {
            Id        = plan.Id,
            UserId    = userId,
            Year      = year,
            Week      = week,
            Entries   = updatedEntries,
            CreatedAt = plan.CreatedAt
        };
        await CreateOrUpdateAsync(updated);
    }

    private static MealPlan MapToDomain(string id, MealPlanDocument doc) => new()
    {
        Id        = id,
        UserId    = doc.UserId,
        Year      = doc.Year,
        Week      = doc.Week,
        Entries   = doc.Entries?.Select(e => new MealPlanEntry { DayOfWeek = e.DayOfWeek, MealType = e.MealType, RecipeId = e.RecipeId, RecipeName = e.RecipeName }).ToList() ?? [],
        CreatedAt = doc.CreatedAt
    };

    private static MealPlanDocument ToDocument(MealPlan p) => new()
    {
        UserId    = p.UserId,
        Year      = p.Year,
        Week      = p.Week,
        Entries   = p.Entries.Select(e => new MealPlanEntryDocument { DayOfWeek = e.DayOfWeek, MealType = e.MealType, RecipeId = e.RecipeId, RecipeName = e.RecipeName }).ToList(),
        CreatedAt = p.CreatedAt
    };
}

[FirestoreData] internal sealed class MealPlanDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("year")]      public int Year { get; set; }
    [FirestoreProperty("week")]      public int Week { get; set; }
    [FirestoreProperty("entries")]   public List<MealPlanEntryDocument>? Entries { get; set; }
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}

[FirestoreData] internal sealed class MealPlanEntryDocument
{
    [FirestoreProperty("dayOfWeek")]  public string DayOfWeek { get; set; } = string.Empty;
    [FirestoreProperty("mealType")]   public string MealType { get; set; } = string.Empty;
    [FirestoreProperty("recipeId")]   public string RecipeId { get; set; } = string.Empty;
    [FirestoreProperty("recipeName")] public string RecipeName { get; set; } = string.Empty;
}