namespace SmartQuiz.Application.Configuration;

public class GeminiOptions
{
    public const string SectionName = "Gemini";

    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gemini-2.5-flash";
    public int NumberOfQuestions { get; set; } = 20;
    public bool Enabled { get; set; } = true;
    public int CacheDurationDays { get; set; } = 30;
    public string DifficultyMix { get; set; } = "Easy,Medium,Hard";
    public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com";
}
