using GermanLearning.Application.DTOs.Exercises;
using GermanLearning.Domain.Enums;
using MediatR;

namespace GermanLearning.Application.Features.Exercises.Commands;

public record GenerateExerciseCommand(
    ExerciseType Type,
    DifficultyLevel Difficulty,
    WordType? WordTypeFilter = null,
    string? TopicFilter = null,
    int? QuestionCount = null) : IRequest<ExerciseDto>;