using GermanLearning.Application.DTOs.Exercises;
using GermanLearning.Application.Features.Exercises.Commands;
using GermanLearning.Application.Features.Exercises.Queries;

namespace GermanLearning.Application.Interfaces;

public interface IExerciseService
{
    Task<ExerciseDto> GenerateExerciseAsync(GenerateExerciseCommand command);
    Task<ExerciseResultDto> SaveExerciseResultAsync(SaveExerciseResultCommand command);
    Task<ExerciseDto> GetExerciseByIdAsync(Guid id);
    Task<List<ExerciseResultDto>> GetExerciseHistoryAsync(GetExerciseHistoryQuery query);
}