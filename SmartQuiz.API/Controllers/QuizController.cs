using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartQuiz.Infrastructure.Services;
using SmartQuiz.Shared.Dtos.Quiz;

namespace SmartQuiz.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private const int QuestionsPerQuiz = 20;
    private readonly GeminiQuestionGenerator _generator;

    public QuizController(GeminiQuestionGenerator generator)
    {
        _generator = generator;
    }

    [HttpPost("start")]
    public async Task<ActionResult<object>> StartQuiz([FromBody] QuizStartRequestDto request, CancellationToken cancellationToken)
    {
        var className = string.IsNullOrWhiteSpace(request.ClassName) ? "Class 6" : request.ClassName.Trim();
        var subject = string.IsNullOrWhiteSpace(request.Subject) ? "Mathematics" : request.Subject.Trim();
        List<QuizQuestionDto> questions;
        var source = "Fallback";

        try
        {
            var generated = await _generator.GenerateAsync(request.Board, className, subject, QuestionsPerQuiz, cancellationToken);
            questions = generated
                .GroupBy(question => question.QuestionText.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(group => ToDto(group.First(), className, subject))
                .Take(QuestionsPerQuiz)
                .ToList();

            if (questions.Count == QuestionsPerQuiz)
            {
                source = "Gemini";
            }
            else
            {
                questions = CreateFallbackQuestions(className, subject);
            }
        }
        catch
        {
            questions = CreateFallbackQuestions(className, subject);
        }

        return Ok(new
        {
            studentName = request.StudentName.Trim(),
            board = request.Board,
            className,
            subject,
            questionCount = QuestionsPerQuiz,
            source,
            questions
        });
    }

    private static QuizQuestionDto ToDto(SmartQuiz.Domain.Entities.Question question, string className, string subject)
    {
        return new QuizQuestionDto
        {
            Question = question.QuestionText,
            OptionA = question.OptionA,
            OptionB = question.OptionB,
            OptionC = question.OptionC,
            OptionD = question.OptionD,
            CorrectAnswer = question.CorrectAnswer,
            Difficulty = question.DifficultyLevel.ToString(),
            ImagePrompt = question.ImagePrompt,
            ImageAltText = question.ImageAltText,
            IsImageQuestion = question.IsImageQuestion,
            ImageUrl = BuildImageUrl(question.ImagePrompt, question.IsImageQuestion)
        };
    }

    private static List<QuizQuestionDto> CreateFallbackQuestions(string className, string subject)
    {
        var imageUrls = new[]
        {
            "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?auto=format&fit=crop&w=800&q=80",
            "https://images.unsplash.com/photo-1513258496099-48168024aec0?auto=format&fit=crop&w=800&q=80",
            "https://images.unsplash.com/photo-1466692476868-aef1dfb1e735?auto=format&fit=crop&w=800&q=80",
            "https://images.unsplash.com/photo-1455390582262-044cdead277a?auto=format&fit=crop&w=800&q=80"
        };

        return Enumerable.Range(1, QuestionsPerQuiz).Select(index => new QuizQuestionDto
        {
            Question = $"{subject} challenge {index}: choose the best answer for this {className} learning puzzle.",
            OptionA = $"Choice {index}A",
            OptionB = $"Choice {index}B",
            OptionC = $"Choice {index}C",
            OptionD = $"Choice {index}D",
            CorrectAnswer = $"Choice {index}B",
            Difficulty = index <= 8 ? "Easy" : index <= 15 ? "Medium" : "Hard",
            IsImageQuestion = className.Equals("Class 6", StringComparison.OrdinalIgnoreCase),
            ImagePrompt = className.Equals("Class 6", StringComparison.OrdinalIgnoreCase)
                ? $"A friendly child-focused illustration connected to {subject} challenge {index}"
                : string.Empty,
            ImageAltText = className.Equals("Class 6", StringComparison.OrdinalIgnoreCase)
                ? $"Illustration for {subject} challenge {index}"
                : string.Empty,
            ImageUrl = className.Equals("Class 6", StringComparison.OrdinalIgnoreCase) || index <= 4
                ? imageUrls[(index - 1) % imageUrls.Length]
                : null
        }).ToList();
    }

    private static string? BuildImageUrl(string imagePrompt, bool isImageQuestion)
    {
        if (!isImageQuestion || string.IsNullOrWhiteSpace(imagePrompt))
        {
            return null;
        }

        var query = Uri.EscapeDataString(imagePrompt);
        return $"https://image.pollinations.ai/prompt/{query}?width=800&height=500&nologo=true";
    }
}
