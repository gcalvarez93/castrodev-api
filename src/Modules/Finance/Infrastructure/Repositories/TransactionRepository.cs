// Path: src/Modules/Finance/Infrastructure/Repositories/TransactionRepository.cs
using Api.Modules.Finance.Domain.Repositories;
using Google.Cloud.Firestore;
using DomainTransaction = Api.Modules.Finance.Domain.Entities.Transaction;

namespace Api.Modules.Finance.Infrastructure.Repositories;

public sealed class TransactionRepository(FirestoreDb db) : ITransactionRepository
{
    private CollectionReference Collection => db.Collection("transactions");

    public async Task<DomainTransaction?> GetByIdAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var transaction = doc.ConvertTo<TransactionDocument>();
        if (transaction.UserId != userId) return null;

        return MapToDomain(doc.Id, transaction);
    }

    public async Task<IEnumerable<DomainTransaction>> GetAllAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .OrderByDescending("date")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<TransactionDocument>()));
    }

    public async Task<IEnumerable<DomainTransaction>> GetByMonthAsync(string userId, int year, int month)
    {
        var start = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddMonths(1);

        var query = await Collection
            .WhereEqualTo("userId", userId)
            .WhereGreaterThanOrEqualTo("date", start)
            .WhereLessThan("date", end)
            .OrderByDescending("date")
            .GetSnapshotAsync();

        return query.Documents.Select(d => MapToDomain(d.Id, d.ConvertTo<TransactionDocument>()));
    }

    public async Task<string> CreateAsync(DomainTransaction transaction)
    {
        var doc = Collection.Document();
        await doc.SetAsync(new TransactionDocument
        {
            UserId = transaction.UserId,
            Amount = (double)transaction.Amount,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            Description = transaction.Description,
            Date = transaction.Date,
            CreatedAt = transaction.CreatedAt
        });
        return doc.Id;
    }

    public async Task UpdateAsync(DomainTransaction transaction)
    {
        await Collection.Document(transaction.Id).SetAsync(new TransactionDocument
        {
            UserId = transaction.UserId,
            Amount = (double)transaction.Amount,
            Type = transaction.Type,
            CategoryId = transaction.CategoryId,
            Description = transaction.Description,
            Date = transaction.Date,
            CreatedAt = transaction.CreatedAt
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;

        var transaction = doc.ConvertTo<TransactionDocument>();
        if (transaction.UserId != userId) return;

        await Collection.Document(id).DeleteAsync();
    }

    public async Task<decimal> GetBalanceAsync(string userId)
    {
        var query = await Collection
            .WhereEqualTo("userId", userId)
            .GetSnapshotAsync();

        return query.Documents.Sum(d =>
        {
            var t = d.ConvertTo<TransactionDocument>();
            return t.Type == "income" ? (decimal)t.Amount : -(decimal)t.Amount;
        });
    }

    private static DomainTransaction MapToDomain(string id, TransactionDocument doc) => new()
    {
        Id = id,
        UserId = doc.UserId,
        Amount = (decimal)doc.Amount,
        Type = doc.Type,
        CategoryId = doc.CategoryId,
        Description = doc.Description,
        Date = doc.Date,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class TransactionDocument
{
    [FirestoreProperty("userId")]      public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("amount")]      public double Amount { get; set; }
    [FirestoreProperty("type")]        public string Type { get; set; } = string.Empty;
    [FirestoreProperty("categoryId")]  public string CategoryId { get; set; } = string.Empty;
    [FirestoreProperty("description")] public string Description { get; set; } = string.Empty;
    [FirestoreProperty("date")]        public DateTime Date { get; set; }
    [FirestoreProperty("createdAt")]   public DateTime CreatedAt { get; set; }
}