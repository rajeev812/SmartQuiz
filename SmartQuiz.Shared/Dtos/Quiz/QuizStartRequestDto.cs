namespace SmartQuiz.Shared.Dtos.Quiz;

public class QuizStartRequestDto
{
    public string StudentName { get; set; } = string.Empty;
    public string Board { get; set; } = "CBSE";
    public string ClassName { get; set; } = "Class 6";
    public string Subject { get; set; } = "Mathematics";
    public int QuestionCount { get; set; } = 5;
}
