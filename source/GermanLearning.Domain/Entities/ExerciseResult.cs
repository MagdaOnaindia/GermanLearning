using System;

namespace GermanLearning.Domain.Entities;

public class ExerciseResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    public ExerciseResult(Guid exerciseId, int correctAnswers, int totalQuestions)
    {
        ExerciseId = exerciseId;
        CorrectAnswers = correctAnswers;
        TotalQuestions = totalQuestions;
    }
}