// Path: src/Modules/WorkoutTracker/Application/UseCases/SearchExternalExercises/SearchExternalExercisesHandler.cs
using System.Text.Json;
using ErrorOr; using MediatR;
using Api.Modules.WorkoutTracker.Application.DTOs;
namespace Api.Modules.WorkoutTracker.Application.UseCases.SearchExternalExercises;
public sealed class SearchExternalExercisesHandler(
    IHttpClientFactory httpClientFactory,
    ILogger<SearchExternalExercisesHandler> logger)
    : IRequestHandler<SearchExternalExercisesQuery, ErrorOr<List<ExternalExerciseDto>>>
{
    public async Task<ErrorOr<List<ExternalExerciseDto>>> Handle(SearchExternalExercisesQuery request, CancellationToken ct)
    {
        var client = httpClientFactory.CreateClient("Wger");
        var url = "exercise/search/?format=json&language=2";
        if (!string.IsNullOrWhiteSpace(request.Name))
            url += $"&term={Uri.EscapeDataString(request.Name)}";

        var response = await client.GetAsync(url, ct);
        if (!response.IsSuccessStatusCode)
            return Error.Failure("ExternalApi.Failed", "Failed to fetch exercises.");

        var json    = await response.Content.ReadAsStringAsync(ct);
        var doc     = JsonDocument.Parse(json);
        var results = new List<ExternalExerciseDto>();

        if (doc.RootElement.TryGetProperty("suggestions", out var suggestions))
        {
            foreach (var item in suggestions.EnumerateArray())
            {
                var data = item.GetProperty("data");
                results.Add(new ExternalExerciseDto(
                    data.GetProperty("id").GetRawText(),
                    data.GetProperty("name").GetString() ?? "",
                    data.TryGetProperty("category", out var cat) ? cat.GetString() ?? "" : "",
                    "",
                    "",
                    "",
                    null
                ));
            }
        }

        logger.LogInformation("Found {Count} external exercises", results.Count);
        return results;
    }
}