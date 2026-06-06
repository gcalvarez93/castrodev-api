// Path: /Users/gabrielcastro/api/Program.cs
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api.Common.Domain.Repositories;
using Api.Common.Infrastructure.Repositories;
using Api.Common.Presentation;
using Api.Modules.Finance.Domain.Repositories;
using Api.Modules.Finance.Infrastructure.Repositories;
using Api.Modules.Finance.Presentation;
using Api.Modules.Habits.Domain.Repositories;
using Api.Modules.Habits.Infrastructure.Repositories;
using Api.Modules.Habits.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Firebase Admin SDK
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.GetApplicationDefault()
});

// Firestore
builder.Services.AddSingleton(_ =>
    FirestoreDb.Create(builder.Configuration["Firebase:ProjectId"]));

// Repositories
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{builder.Configuration["Firebase:ProjectId"]}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{builder.Configuration["Firebase:ProjectId"]}",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Firebase:ProjectId"],
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IHabitRepository, HabitRepository>();
builder.Services.AddScoped<IHabitCompletionRepository, HabitCompletionRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("CastroDev API running")).AllowAnonymous();
app.MapUserEndpoints();
app.MapTransactionEndpoints();
app.MapCategoryEndpoints();
app.MapBudgetEndpoints();
app.MapHabitEndpoints();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");