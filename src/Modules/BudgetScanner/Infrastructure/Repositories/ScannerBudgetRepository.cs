// Path: src/Modules/BudgetScanner/Infrastructure/Repositories/ScannerBudgetRepository.cs
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.BudgetScanner.Infrastructure.Repositories;

public sealed class ScannerBudgetRepository(FirestoreDb db) : IScannerBudgetRepository
{
    private CollectionReference Collection => db.Collection("budgets_budgetscanner");

    public async Task<ScannerBudget?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var data = doc.ConvertTo<ScannerBudgetDocument>();
        if (data.UserId != userId) return null;

        return MapToDomain(doc.Id, data);
    }

    public async Task<IEnumerable<ScannerBudget>> GetAllAsync(string userId)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .OrderBy("createdAt")
            .GetSnapshotAsync();

        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<ScannerBudgetDocument>()));
    }

    public async Task<string> CreateAsync(ScannerBudget budget)
    {
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(budget));
        return doc.Id;
    }

    public async Task UpdateAsync(ScannerBudget budget)
    {
        await Collection.Document(budget.Id).SetAsync(ToDocument(budget), SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        if (doc.ConvertTo<ScannerBudgetDocument>().UserId != userId) return;
        await Collection.Document(id).DeleteAsync();
    }

    public async Task ResetRecurringBudgetsAsync(string userId)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("isRecurring", true)
            .GetSnapshotAsync();

        var batch = db.StartBatch();
        foreach (var doc in snapshot.Documents)
            batch.Update(doc.Reference, new Dictionary<string, object> { ["spent"] = 0m });

        await batch.CommitAsync();
    }

    private static ScannerBudget MapToDomain(string id, ScannerBudgetDocument doc) => new()
    {
        Id             = id,
        UserId         = doc.UserId,
        Name           = doc.Name,
        Category       = doc.Category,
        Limit          = doc.Limit,
        Color          = doc.Color,
        Currency       = doc.Currency,
        IsRecurring    = doc.IsRecurring,
        AlertThreshold = doc.AlertThreshold,
        CreatedAt      = doc.CreatedAt
    };

    private static ScannerBudgetDocument ToDocument(ScannerBudget b) => new()
    {
        UserId         = b.UserId,
        Name           = b.Name,
        Category       = b.Category,
        Limit          = b.Limit,
        Color          = b.Color,
        Currency       = b.Currency,
        IsRecurring    = b.IsRecurring,
        AlertThreshold = b.AlertThreshold,
        CreatedAt      = b.CreatedAt
    };
}

[FirestoreData]
internal sealed class ScannerBudgetDocument
{
    [FirestoreProperty("userId")]         public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]           public string Name { get; set; } = string.Empty;
    [FirestoreProperty("category")]       public string Category { get; set; } = string.Empty;
    [FirestoreProperty("limit")]          public decimal Limit { get; set; }
    [FirestoreProperty("color")]          public string Color { get; set; } = string.Empty;
    [FirestoreProperty("currency")]       public string Currency { get; set; } = "EUR";
    [FirestoreProperty("isRecurring")]    public bool IsRecurring { get; set; }
    [FirestoreProperty("alertThreshold")] public int AlertThreshold { get; set; } = 80;
    [FirestoreProperty("createdAt")]      public DateTime CreatedAt { get; set; }
}