using GermanLearning.Application.DTOs.Exercises;
using GermanLearning.Domain.Enums;
using MediatR;

namespace GermanLearning.Application.Features.Exercises.Queries;

public record GetExerciseHistoryQuery(
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    ExerciseType? TypeFilter = null) : IRequest<List<ExerciseResultDto>>;