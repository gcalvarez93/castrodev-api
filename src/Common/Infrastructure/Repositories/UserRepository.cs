// Path: src/Common/Infrastructure/Repositories/UserRepository.cs
using Api.Common.Domain.Entities;
using Api.Common.Domain.Repositories;
using Google.Cloud.Firestore;

namespace Api.Common.Infrastructure.Repositories;

public sealed class UserRepository(FirestoreDb db) : IUserRepository
{
    private CollectionReference Collection => db.Collection("users");

    public async Task<User?> GetByIdAsync(string id)
    {
        var doc = await Collection.Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var user = doc.ConvertTo<UserDocument>();
        return MapToDomain(doc.Id, user);
    }

    public async Task CreateAsync(User user)
    {
        await Collection.Document(user.Id).SetAsync(new UserDocument
        {
            Name = user.Name,
            Email = user.Email,
            PhotoUrl = user.PhotoUrl,
            Language = user.Language,
            Notifications = user.Notifications,
            CreatedAt = user.CreatedAt
        });
    }

    public async Task UpdateAsync(User user)
    {
        await Collection.Document(user.Id).SetAsync(new UserDocument
        {
            Name = user.Name,
            Email = user.Email,
            PhotoUrl = user.PhotoUrl,
            Language = user.Language,
            Notifications = user.Notifications,
            CreatedAt = user.CreatedAt
        }, SetOptions.Overwrite);
    }

    private static User MapToDomain(string id, UserDocument doc) => new()
    {
        Id = id,
        Name = doc.Name,
        Email = doc.Email,
        PhotoUrl = doc.PhotoUrl,
        Language = doc.Language,
        Notifications = doc.Notifications,
        CreatedAt = doc.CreatedAt
    };
}

[FirestoreData]
internal sealed class UserDocument
{
    [FirestoreProperty("name")]          public string Name { get; set; } = string.Empty;
    [FirestoreProperty("email")]         public string Email { get; set; } = string.Empty;
    [FirestoreProperty("photoUrl")]      public string PhotoUrl { get; set; } = string.Empty;
    [FirestoreProperty("language")]      public string Language { get; set; } = "es";
    [FirestoreProperty("notifications")] public bool Notifications { get; set; } = true;
    [FirestoreProperty("createdAt")]     public DateTime CreatedAt { get; set; }
}