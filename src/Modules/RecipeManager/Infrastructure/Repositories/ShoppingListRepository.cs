// Path: src/Modules/RecipeManager/Infrastructure/Repositories/ShoppingListRepository.cs
using Api.Modules.RecipeManager.Domain.Entities;
using Api.Modules.RecipeManager.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Modules.RecipeManager.Infrastructure.Repositories;

public sealed class ShoppingListRepository(FirestoreDb db) : IShoppingListRepository
{
    private CollectionReference Collection => db.Collection("shoppinglists_recipemanager");

    public async Task<ShoppingList?> GetByWeekAsync(string userId, int year, int week)
    {
        var snapshot = await Collection
            .WhereEqualTo("userId", userId)
            .WhereEqualTo("year", year)
            .WhereEqualTo("week", week)
            .Limit(1)
            .GetSnapshotAsync();

        if (snapshot.Count == 0) return null;
        var doc = snapshot.Documents[0];
        return MapToDomain(doc.Id, doc.ConvertTo<ShoppingListDocument>());
    }

    public async Task<string> CreateOrUpdateAsync(ShoppingList list)
    {
        if (!string.IsNullOrEmpty(list.Id))
        {
            await Collection.Document(list.Id).SetAsync(ToDocument(list), SetOptions.Overwrite);
            return list.Id;
        }
        var doc = Collection.Document();
        await doc.SetAsync(ToDocument(list));
        return doc.Id;
    }

    public async Task ToggleItemAsync(string id, string userId, string itemName)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return;
        var data = doc.ConvertTo<ShoppingListDocument>();
        if (data.UserId != userId) return;

        var items = data.Items ?? [];
        var item  = items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        if (item is null) return;

        item.IsChecked = !item.IsChecked;
        await Collection.Document(id).UpdateAsync("items", items);
    }

    private static ShoppingList MapToDomain(string id, ShoppingListDocument doc) => new()
    {
        Id        = id,
        UserId    = doc.UserId,
        Name      = doc.Name,
        Year      = doc.Year,
        Week      = doc.Week,
        Items     = doc.Items?.Select(i => new ShoppingListItem { Name = i.Name, Amount = i.Amount, Unit = i.Unit, IsChecked = i.IsChecked }).ToList() ?? [],
        CreatedAt = doc.CreatedAt
    };

    private static ShoppingListDocument ToDocument(ShoppingList l) => new()
    {
        UserId    = l.UserId,
        Name      = l.Name,
        Year      = l.Year,
        Week      = l.Week,
        Items     = l.Items.Select(i => new ShoppingListItemDocument { Name = i.Name, Amount = i.Amount, Unit = i.Unit, IsChecked = i.IsChecked }).ToList(),
        CreatedAt = l.CreatedAt
    };
}

[FirestoreData] internal sealed class ShoppingListDocument
{
    [FirestoreProperty("userId")]    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("name")]      public string Name { get; set; } = string.Empty;
    [FirestoreProperty("year")]      public int Year { get; set; }
    [FirestoreProperty("week")]      public int Week { get; set; }
    [FirestoreProperty("items")]     public List<ShoppingListItemDocument>? Items { get; set; }
    [FirestoreProperty("createdAt")] public DateTime CreatedAt { get; set; }
}

[FirestoreData] internal sealed class ShoppingListItemDocument
{
    [FirestoreProperty("name")]      public string Name { get; set; } = string.Empty;
    [FirestoreProperty("amount")]    public string Amount { get; set; } = string.Empty;
    [FirestoreProperty("unit")]      public string Unit { get; set; } = string.Empty;
    [FirestoreProperty("isChecked")] public bool IsChecked { get; set; }
}