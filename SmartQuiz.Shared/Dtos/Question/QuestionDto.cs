using SmartQuiz.Domain.Enums;

namespace SmartQuiz.Shared.Dtos.Question;

public class QuestionDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; }
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; }
    public int Marks { get; set; }
    public bool IsActive { get; set; }
}
