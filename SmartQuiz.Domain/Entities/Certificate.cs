namespace SmartQuiz.Domain.Entities;

public class Certificate : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid AttemptId { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public string? CertificateUrl { get; set; }
    public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public QuizAttempt? Attempt { get; set; }
}
