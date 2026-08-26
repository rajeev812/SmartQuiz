using SmartQuiz.Domain.Entities;

namespace SmartQuiz.Infrastructure.Services;

public class QuestionCacheService
{
    private readonly List<Question> _cache = new();

    public Task<List<Question>> GetCachedQuestionsAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_cache.ToList());
    }

    public Task SaveAsync(IEnumerable<Question> questions, CancellationToken cancellationToken = default)
    {
        _cache.Clear();
        _cache.AddRange(questions);
        return Task.CompletedTask;
    }

    public Task<bool> ShouldGenerateNewQuestionsAsync(int currentCount, int minimumThreshold, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(currentCount < minimumThreshold);
    }
}
