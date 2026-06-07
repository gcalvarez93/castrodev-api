// Path: src/Modules/Finance/Infrastructure/Repositories/BudgetRepository.cs
using Api.Modules.Finance.Domain.Entities;
using Api.Modules.Finance.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.Finance.Infrastructure.Repositories;

public sealed class BudgetRepository(FirestoreDb db) : IBudgetRepository
{
    private CollectionReference Collection => db.Collection("budgets_financetracker");

    public async Task<Budget?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var budget = doc.ConvertTo<BudgetDocument>();
        if (budget.UserId != userId) return null;

        return MapToDomain(doc.Id, budget);
    }

    public async Task<IEnumerable<Budget>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderByDescending("month")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<BudgetDocument>()));
    }

    public async Task<IEnumerable<Budget>> GetByMonthAsync(string userId, string month)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("month", month)
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<BudgetDocument>()));
    }

    public async Task<string> CreateAsync(Budget budget)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new BudgetDocument
        {
            UserId = budget.UserId,
            CategoryId = budget.CategoryId,
            Amount = (double)budget.Amount,
            Month = budget.Month,
            CreatedAt = budget.CreatedAt
        });
        return doc.Id;
    }

    public async Task UpdateAsync(Budget budget)
    {
        await Collection.Document(budget.Id).SetAsync(new BudgetDocument
        {
            UserId = budget.UserId,
            CategoryId = budget.CategoryId,
            Amount = (double)budget.Amount,
            Month = budget.Month,
            CreatedAt = budget.CreatedAt
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var budget = doc.ConvertTo<BudgetDocument>();
        if (budget.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    private static Budget MapToDomain(string id, BudgetDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        CategoryId = doc.CategoryId,
        Amount = (decimal)doc.Amount,
        Month = doc.Month,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class BudgetDocument
{
    [FirestoreProperty("userId")]     public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("categoryId")] public string CategoryId { get; set; } = string.Empty;
    [FirestoreProperty("amount")]     public double Amount { get; set; }
    [FirestoreProperty("month")]      public string Month { get; set; } = string.Empty;
    [FirestoreProperty("createdAt")]  public DateTime CreatedAt { get; set; }
}