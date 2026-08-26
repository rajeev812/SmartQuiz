namespace SmartQuiz.Application.Interfaces.Services;

public interface IGeminiService
{
    Task<string> GenerateQuestionsAsync(string board, string className, string subject, int questionCount, CancellationToken cancellationToken = default);
    Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default);
}
