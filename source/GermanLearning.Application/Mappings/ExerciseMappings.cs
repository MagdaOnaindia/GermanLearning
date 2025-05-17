using GermanLearning.Application.DTOs.Exercises;
using GermanLearning.Domain.Entities;

namespace GermanLearning.Application.Mappings;

public static class ExerciseMappings
{
    public static ExerciseDto ToDto(this Exercise exercise)
    {
        return new ExerciseDto(
            exercise.Id,
            exercise.Type,
            exercise.Difficulty,
            exercise.Words.Select(w => w.ToDto()).ToList(),
            exercise.GeneratedAt);
    }

    public static ExerciseResultDto ToDto(this ExerciseResult result)
    {
        return new ExerciseResultDto(
            result.Id,
            result.ExerciseId,
            result.CorrectAnswers,
            result.TotalQuestions,
            result.CompletedAt,
            Math.Round((double)result.CorrectAnswers / result.TotalQuestions * 100, 2));
    }
}