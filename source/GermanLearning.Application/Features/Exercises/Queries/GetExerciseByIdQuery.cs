using GermanLearning.Application.DTOs.Exercises;
using MediatR;

namespace GermanLearning.Application.Features.Exercises.Queries;

public record GetExerciseByIdQuery(Guid Id) : IRequest<ExerciseDto>;