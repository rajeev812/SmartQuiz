namespace SmartQuiz.Domain.Entities;

public class Board : BaseEntity
{
    public string BoardName { get; set; } = string.Empty;
    public ICollection<SchoolClass> Classes { get; set; } = new List<SchoolClass>();
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
