using SmartQuiz.Domain.Enums;

namespace SmartQuiz.Domain.Entities;

public class QuizAttempt : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BoardId { get; set; }
    public Guid ClassId { get; set; }
    public Guid SubjectId { get; set; }
    public int TotalQuestions { get; set; }
    public int TotalMarks { get; set; }
    public int ObtainedMarks { get; set; }
    public decimal Percentage { get; set; }
    public QuizStatus Status { get; set; } = QuizStatus.Pending;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime AttemptDate { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Board? Board { get; set; }
    public SchoolClass? Class { get; set; }
    public Subject? Subject { get; set; }
    public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    public Certificate? Certificate { get; set; }
}
