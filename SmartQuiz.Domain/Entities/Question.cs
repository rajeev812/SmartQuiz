using SmartQuiz.Domain.Enums;

namespace SmartQuiz.Domain.Entities;

public class Question : BaseEntity
{
    public Guid BoardId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public QuestionType QuestionType { get; set; } = QuestionType.MultipleChoice;
    public string OptionA { get; set; } = string.Empty;
    public string OptionB { get; set; } = string.Empty;
    public string OptionC { get; set; } = string.Empty;
    public string OptionD { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Medium;
    public int Marks { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public string Source { get; set; } = "Database";
    public bool GeneratedByAI { get; set; }
    public DateTime? GeneratedDate { get; set; }
    public string PromptVersion { get; set; } = "v1";
    public string ImagePrompt { get; set; } = string.Empty;
    public string ImageAltText { get; set; } = string.Empty;
    public bool IsImageQuestion { get; set; }

    public Board? Board { get; set; }
    public SchoolClass? Class { get; set; }
    public Subject? Subject { get; set; }
    public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
