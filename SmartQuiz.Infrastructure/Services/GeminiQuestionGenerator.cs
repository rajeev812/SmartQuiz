using System.Text.Json;
using SmartQuiz.Application.Interfaces.Services;
using SmartQuiz.Domain.Entities;

namespace SmartQuiz.Infrastructure.Services;

public class GeminiQuestionGenerator
{
    private readonly IGeminiService _geminiService;
    private readonly QuestionValidationService _validationService;

    public GeminiQuestionGenerator(IGeminiService geminiService, QuestionValidationService validationService)
    {
        _geminiService = geminiService;
        _validationService = validationService;
    }

    public async Task<List<Question>> GenerateAsync(string board, string className, string subject, int questionCount, CancellationToken cancellationToken = default)
    {
        var rawJson = await _geminiService.GenerateQuestionsAsync(board, className, subject, questionCount, cancellationToken);
        var validatedQuestions = _validationService.ValidateAndNormalize(rawJson);

        return validatedQuestions.Select(q => new Question
        {
            QuestionText = q.Question,
            OptionA = q.OptionA,
            OptionB = q.OptionB,
            OptionC = q.OptionC,
            OptionD = q.OptionD,
            CorrectAnswer = q.CorrectAnswer,
            ImagePrompt = q.ImagePrompt,
            ImageAltText = q.ImageAltText,
            IsImageQuestion = q.IsImageQuestion,
            DifficultyLevel = q.Difficulty switch
            {
                "Easy" => SmartQuiz.Domain.Enums.DifficultyLevel.Easy,
                "Medium" => SmartQuiz.Domain.Enums.DifficultyLevel.Medium,
                _ => SmartQuiz.Domain.Enums.DifficultyLevel.Hard
            },
            Source = "Gemini",
            GeneratedByAI = true,
            GeneratedDate = DateTime.UtcNow,
            PromptVersion = "v1",
            IsActive = true,
            Marks = 5,
            QuestionType = SmartQuiz.Domain.Enums.QuestionType.MultipleChoice
        }).ToList();
    }
}
