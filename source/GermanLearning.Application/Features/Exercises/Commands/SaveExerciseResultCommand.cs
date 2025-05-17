using GermanLearning.Application.DTOs.Exercises;
using MediatR;

namespace GermanLearning.Application.Features.Exercises.Commands;

public record SaveExerciseResultCommand(
    Guid ExerciseId,
    int CorrectAnswers,
    int TotalQuestions) : IRequest<ExerciseResultDto>;