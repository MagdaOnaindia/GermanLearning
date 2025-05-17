using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Domain.Repositories.Interfaces;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(Guid id);
    Task<List<Exercise>> GetAllAsync();
    Task AddAsync(Exercise exercise);
    Task SaveAsync();
    Task<List<ExerciseResult>> GetResultsByDateRangeAsync(DateTime? fromDate, DateTime? toDate, ExerciseType? typeFilter);
    Task SaveResultAsync(ExerciseResult result);
    Task<ExerciseResult?> GetResultByIdAsync(Guid id);
    Task<Exercise> GenerateExerciseAsync(ExerciseType type, DifficultyLevel difficulty, List<Word> words);
}