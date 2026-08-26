using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SmartQuiz.Application.Configuration;
using SmartQuiz.Application.Interfaces.Services;

namespace SmartQuiz.Infrastructure.Services;

public class GeminiService : IGeminiService
{
    private readonly GeminiOptions _options;
    private readonly HttpClient _httpClient;

    public GeminiService(IOptions<GeminiOptions> options, HttpClient httpClient)
    {
        _options = options.Value;
        _httpClient = httpClient;
    }

    public async Task<string> GenerateQuestionsAsync(string board, string className, string subject, int questionCount, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled || string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("Gemini generation is disabled or the API key is not configured.");
        }

        var prompt = new PromptBuilder().Build(board, className, subject, questionCount);
        var url = $"{_options.BaseUrl}/v1beta/models/{_options.Model}:generateContent";

        var requestBody = new
        {
            generationConfig = new
            {
                responseMimeType = "application/json",
                temperature = 0.8
            },
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("x-goog-api-key", _options.ApiKey);
        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorText = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Gemini API call failed: {response.StatusCode}. {errorText}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("candidates", out var candidates) && candidates.ValueKind == JsonValueKind.Array && candidates.GetArrayLength() > 0)
        {
            var text = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? string.Empty;
        }

        throw new InvalidOperationException("Gemini returned an unexpected payload.");
    }

    public Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_options.Enabled);
    }
}
