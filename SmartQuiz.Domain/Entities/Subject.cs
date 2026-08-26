namespace SmartQuiz.Domain.Entities;

public class Subject : BaseEntity
{
    public string SubjectName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
