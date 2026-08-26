namespace SmartQuiz.Infrastructure.Services;

public class PromptBuilder
{
    public string Build(string board, string className, string subject, int questionCount)
    {
        return $@"Generate {questionCount} curriculum-aligned multiple-choice questions suitable for:

Board: {board}
Class: {className}
Subject: {subject}

Requirements:
- Follow {board} syllabus level
- Mix Easy, Medium and Hard questions
- Four options per question
- Include the correct answer
- All {questionCount} questions must be unique in wording and concept
- Use student-friendly language
- For Classes 1 through 6, make questions playful and connect them to familiar school, home, sports, nature or everyday-life situations
- For Classes 1 through 6, make at least 12 questions image-based and set isImageQuestion to true. The image must directly show the object, person, place, animal, experiment or action asked about.
- For Classes 1 through 6, image questions may ask learners to identify a clearly named person, sport, animal, landmark or object shown in the image. Put the exact correct identity in imagePrompt and imageAltText, and include four plausible identity options.
- For classes above Class 6, include an imagePrompt only when it genuinely helps explain the concept, for no more than 5 questions
- Set isImageQuestion to false when the image is not required to answer the question
- For every image question, imagePrompt must describe the exact visual evidence needed to answer it, and imageAltText must state what the learner should see
- Never use decorative, generic classroom, stock or unrelated images
- Options must be plausible, different from one another, and exactly one option must be correct
- Return JSON only

Return format:
{{
  ""questions"": [
    {{
      ""question"": """",
      ""optionA"": """",
      ""optionB"": """",
      ""optionC"": """",
      ""optionD"": """",
      ""correctAnswer"": """",
      ""difficulty"": """",
      ""imagePrompt"": """"
      ,""imageAltText"": """",
      ""isImageQuestion"": false
    }}
  ]
}}";
    }
}
