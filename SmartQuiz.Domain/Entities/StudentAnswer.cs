namespace SmartQuiz.Domain.Entities;

public class StudentAnswer : BaseEntity
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public string? SelectedAnswer { get; set; }
    public bool IsCorrect { get; set; }

    public QuizAttempt? Attempt { get; set; }
    public Question? Question { get; set; }
}
