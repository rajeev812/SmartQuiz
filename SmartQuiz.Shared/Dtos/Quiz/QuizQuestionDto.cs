namespace SmartQuiz.Shared.Dtos.Quiz;

public class QuizQuestionDto
{
    public string Question { get; set; } = string.Empty;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Easy";
    public string? ImageUrl { get; set; }
    public string? ImagePrompt { get; set; }
    public string? ImageAltText { get; set; }
    public bool IsImageQuestion { get; set; }
}
