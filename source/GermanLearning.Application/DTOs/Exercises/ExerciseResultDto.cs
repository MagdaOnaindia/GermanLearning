namespace GermanLearning.Application.DTOs.Exercises;

public record ExerciseResultDto(
    Guid Id,
    Guid ExerciseId,
    int CorrectAnswers,
    int TotalQuestions,
    DateTime CompletedAt,
    double ScorePercentage);