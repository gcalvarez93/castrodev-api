// Path: src/Modules/RecipeManager/Application/UseCases/SearchExternalRecipesHandler.cs
using System.Text.Json;
using ErrorOr;
using MediatR;
using Api.Modules.RecipeManager.Application.DTOs;

namespace Api.Modules.RecipeManager.Application.UseCases;

public sealed class SearchExternalRecipesHandler(
    IHttpClientFactory httpClientFactory,
    ILogger<SearchExternalRecipesHandler> logger
) : IRequestHandler<SearchExternalRecipesQuery, ErrorOr<List<ExternalRecipeDto>>>
{
    public async Task<ErrorOr<List<ExternalRecipeDto>>> Handle(
        SearchExternalRecipesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return Error.Validation("Search.EmptyQuery", "Search query cannot be empty.");

        var client   = httpClientFactory.CreateClient("TheMealDB");
        var response = await client.GetAsync($"search.php?s={Uri.EscapeDataString(request.Query)}", cancellationToken);

        if (!response.IsSuccessStatusCode)
            return Error.Failure("ExternalApi.Failed", "Failed to fetch recipes from external API.");

        var json    = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc     = JsonDocument.Parse(json);
        var meals   = doc.RootElement.GetProperty("meals");

        if (meals.ValueKind == JsonValueKind.Null)
            return new List<ExternalRecipeDto>();

        var results = new List<ExternalRecipeDto>();
        foreach (var meal in meals.EnumerateArray())
        {
            var ingredients = new List<IngredientDto>();
            for (int i = 1; i <= 20; i++)
            {
                var name   = meal.GetProperty($"strIngredient{i}").GetString();
                var amount = meal.GetProperty($"strMeasure{i}").GetString();
                if (string.IsNullOrWhiteSpace(name)) break;
                ingredients.Add(new IngredientDto(name, amount ?? "", ""));
            }

            results.Add(new ExternalRecipeDto(
                meal.GetProperty("idMeal").GetString() ?? "",
                meal.GetProperty("strMeal").GetString() ?? "",
                meal.GetProperty("strCategory").GetString() ?? "",
                meal.GetProperty("strArea").GetString() ?? "",
                meal.GetProperty("strInstructions").GetString() ?? "",
                meal.GetProperty("strMealThumb").GetString() ?? "",
                ingredients,
                meal.GetProperty("strYoutube").GetString()
            ));
        }

        logger.LogInformation("Found {Count} external recipes for query '{Query}'", results.Count, request.Query);
        return results;
    }
}