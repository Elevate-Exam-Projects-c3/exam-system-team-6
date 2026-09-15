namespace exam_system.Features.Quizzes.AdminManageQuestions.Commands;

public record QuestionOptionInput(
    string OptionText,
    bool IsCorrect
);
