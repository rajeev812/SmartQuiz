namespace SmartQuiz.Domain.Entities;

public class SchoolClass : BaseEntity
{
    public string ClassName { get; set; } = string.Empty;
    public Guid BoardId { get; set; }
    public Board? Board { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
