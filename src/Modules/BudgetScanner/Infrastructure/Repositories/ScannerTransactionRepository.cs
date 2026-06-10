// Path: src/Modules/BudgetScanner/Infrastructure/Repositories/ScannerTransactionRepository.cs
using Api.Modules.BudgetScanner.Domain.Entities;
using Api.Modules.BudgetScanner.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.BudgetScanner.Infrastructure.Repositories;

public sealed class ScannerTransactionRepository(FirestoreDb db) : IScannerTransactionRepository
{
    private CollectionReference Collection => db.Collection("transactions_budgetscanner");

    public async Task<ScannerTransaction?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var data = doc.ConvertTo<ScannerTransactionDocument>();
        if (data.UserId != userId) return null;

        return MapToDomain(doc.Id, data);
    }

    public async Task<IEnumerable<ScannerTransaction>> GetAllAsync(string userId, string? budgetId = null)
    {
        Query query = Collection.WhereEqualTo("userId", userId);

        if (!string.IsNullOrWhiteSpace(budgetId))
            query = query.WhereEqualTo("budgetId", budgetId);

        var snapshot = await query.OrderByDescending("date").GetSnapshotAsync();
        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<ScannerTransactionDocument>()));
    }

    public async Task<IEnumerable<ScannerTransaction>> GetByMonthAsync(string userId, int year, int month)
    {
        var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var to   = from.AddMonths(1).AddTicks(-1);
        return await GetByDateRangeAsync(userId, from, to);
    }

    public async Task<IEnumerable<ScannerTransaction>> GetByDateRangeAsync(
        string userId, DateTime from, DateTime to)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereGreaterThanOrEqualTo("date", from)
            .WhereLessThanOrEqualTo("date", to)
            .OrderByDescending("date")
            .GetSnapshotAsync();

        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<ScannerTransactionDocument>()));
    }

    public async Task<IEnumerable<ScannerTransaction>> GetByTagAsync(string userId, string tag)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereArrayContains("tags", tag)
            .OrderByDescending("date")
            .GetSnapshotAsync();

        return snapshot.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<ScannerTransactionDocument>()));
    }

    public async Task<decimal> GetSpentByBudgetAsync(string budgetId, string userId, int year, int month)
    {
        var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var to   = from.AddMonths(1).AddTicks(-1);

        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("budgetId", budgetId)
            .WhereGreaterThanOrEqualTo("date", from)
            .WhereLessThanOrEqualTo("date", to)
            .GetSnapshotAsync();

        return snapshot.Documents
            .Sum(d => d.ConvertTo<ScannerTransactionDocument>().Amount);
    }

    public async Task<string> CreateAsync(ScannerTransaction transaction)
    {
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(transaction));
        return doc.Id;
    }

    public async Task UpdateAsync(ScannerTransaction transaction)
    {
        await Collection.Document(transaction.Id).SetAsync(ToDocument(transaction), SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        if (doc.ConvertTo<ScannerTransactionDocument>().UserId != userId) return;
        await Collection.Document(id).DeleteAsync();
    }

    private static ScannerTransaction MapToDomain(string id, ScannerTransactionDocument doc) => new()
    {
        Id              = id,
        UserId          = doc.UserId,
        BudgetId        = doc.BudgetId,
        Category        = doc.Category,
        Amount          = doc.Amount,
        Currency        = doc.Currency,
        Description     = doc.Description,
        Notes           = doc.Notes,
        Tags            = doc.Tags ?? [],
        ReceiptImageUrl = doc.ReceiptImageUrl,
        Merchant        = doc.Merchant,
        Date            = doc.Date,
        IsScanned       = doc.IsScanned,
        CreatedAt       = doc.CreatedAt
    };

    private static ScannerTransactionDocument ToDocument(ScannerTransaction t) => new()
    {
        UserId          = t.UserId,
        BudgetId        = t.BudgetId,
        Category        = t.Category,
        Amount          = t.Amount,
        Currency        = t.Currency,
        Description     = t.Description,
        Notes           = t.Notes,
        Tags            = t.Tags,
        ReceiptImageUrl = t.ReceiptImageUrl,
        Merchant        = t.Merchant,
        Date            = t.Date,
        IsScanned       = t.IsScanned,
        CreatedAt       = t.CreatedAt
    };
}

[FirestoreData]
internal sealed class ScannerTransactionDocument
{
    [FirestoreProperty("userId")]          public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("budgetId")]        public string BudgetId { get; set; } = string.Empty;
    [FirestoreProperty("category")]        public string Category { get; set; } = string.Empty;
    [FirestoreProperty("amount")]          public decimal Amount { get; set; }
    [FirestoreProperty("currency")]        public string Currency { get; set; } = "EUR";
    [FirestoreProperty("description")]     public string Description { get; set; } = string.Empty;
    [FirestoreProperty("notes")]           public string Notes { get; set; } = string.Empty;
    [FirestoreProperty("tags")]            public List<string>? Tags { get; set; }
    [FirestoreProperty("receiptImageUrl")] public string? ReceiptImageUrl { get; set; }
    [FirestoreProperty("merchant")]        public string? Merchant { get; set; }
    [FirestoreProperty("date")]            public DateTime Date { get; set; }
    [FirestoreProperty("isScanned")]       public bool IsScanned { get; set; }
    [FirestoreProperty("createdAt")]       public DateTime CreatedAt { get; set; }
}