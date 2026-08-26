using System.Text.Json;

namespace SmartQuiz.Infrastructure.Services;

public class QuestionValidationService
{
    public List<QuestionResponse> ValidateAndNormalize(string rawJson)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new InvalidOperationException("Gemini returned an empty response.");
        }

        try
        {
            rawJson = ExtractJson(rawJson);
            using var document = JsonDocument.Parse(rawJson);
            if (!document.RootElement.TryGetProperty("questions", out var questionsElement) || questionsElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Question payload does not contain a valid questions array.");
            }

            var result = new List<QuestionResponse>();

            foreach (var item in questionsElement.EnumerateArray())
            {
                var question = new QuestionResponse
                {
                    Question = ReadString(item, "question"),
                    OptionA = ReadString(item, "optionA", "option_a", "A"),
                    OptionB = ReadString(item, "optionB", "option_b", "B"),
                    OptionC = ReadString(item, "optionC", "option_c", "C"),
                    OptionD = ReadString(item, "optionD", "option_d", "D"),
                    CorrectAnswer = ReadString(item, "correctAnswer", "correct_answer", "answer"),
                    Difficulty = ReadString(item, "difficulty"),
                    ImagePrompt = ReadString(item, "imagePrompt", "image_prompt"),
                    ImageAltText = ReadString(item, "imageAltText", "image_alt_text", "imageDescription"),
                    IsImageQuestion = ReadBoolean(item, "isImageQuestion", "is_image_question")
                };

                question.CorrectAnswer = NormalizeCorrectAnswer(question);

                if (string.IsNullOrWhiteSpace(question.Question) ||
                    string.IsNullOrWhiteSpace(question.OptionA) ||
                    string.IsNullOrWhiteSpace(question.OptionB) ||
                    string.IsNullOrWhiteSpace(question.OptionC) ||
                    string.IsNullOrWhiteSpace(question.OptionD) ||
                    string.IsNullOrWhiteSpace(question.CorrectAnswer))
                {
                    continue;
                }

                var options = new[] { question.OptionA, question.OptionB, question.OptionC, question.OptionD };
                if (options.Distinct(StringComparer.OrdinalIgnoreCase).Count() != 4)
                {
                    continue;
                }

                if (!options.Contains(question.CorrectAnswer, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                result.Add(question);
            }

            return result
                .GroupBy(q => q.Question.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Malformed Gemini JSON response received.", ex);
        }
    }

    private static string ExtractJson(string response)
    {
        var cleaned = response.Trim();
        if (cleaned.StartsWith("```", StringComparison.Ordinal))
        {
            cleaned = cleaned.Replace("```json", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("```", string.Empty, StringComparison.Ordinal)
                .Trim();
        }

        var start = cleaned.IndexOf('{');
        var end = cleaned.LastIndexOf('}');
        return start >= 0 && end > start ? cleaned[start..(end + 1)] : cleaned;
    }

    private static string ReadString(JsonElement item, params string[] names)
    {
        foreach (var name in names)
        {
            if (item.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String)
            {
                return property.GetString() ?? string.Empty;
            }

            var match = item.EnumerateObject().FirstOrDefault(property =>
                string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase));
            if (match.Value.ValueKind == JsonValueKind.String)
            {
                return match.Value.GetString() ?? string.Empty;
            }
        }

        return string.Empty;
    }

    private static bool ReadBoolean(JsonElement item, params string[] names)
    {
        foreach (var name in names)
        {
            var match = item.EnumerateObject().FirstOrDefault(property =>
                string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase));
            if (match.Value.ValueKind == JsonValueKind.True || match.Value.ValueKind == JsonValueKind.False)
            {
                return match.Value.GetBoolean();
            }
        }

        return false;
    }

    private static string NormalizeCorrectAnswer(QuestionResponse question)
    {
        var answer = question.CorrectAnswer.Trim();
        return answer.ToUpperInvariant() switch
        {
            "A" or "OPTION A" or "OPTIONA" => question.OptionA,
            "B" or "OPTION B" or "OPTIONB" => question.OptionB,
            "C" or "OPTION C" or "OPTIONC" => question.OptionC,
            "D" or "OPTION D" or "OPTIOND" => question.OptionD,
            _ => answer
        };
    }
}

public class QuestionResponse
{
    public string Question { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Medium";
    public string ImagePrompt { get; set; } = string.Empty;
    public string ImageAltText { get; set; } = string.Empty;
    public bool IsImageQuestion { get; set; }
}
