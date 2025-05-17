using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Repositories.Interfaces;
using MongoDB.Driver;
using GermanLearning.Infrastructure.Persistence.Contexts;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Infrastructure.Persistence.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly MongoDbContext _context;

    public ExerciseRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Exercise?> GetByIdAsync(Guid id)
    {
        return await _context.Exercises
            .Find(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Exercise>> GetAllAsync()
    {
        return await _context.Exercises
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task<Exercise> GenerateExerciseAsync(ExerciseType type, DifficultyLevel difficulty, List<Word> words)
    {
        var exercise = new Exercise(type, words, difficulty);
        await _context.Exercises.InsertOneAsync(exercise);
        return exercise;
    }

    public async Task AddAsync(Exercise exercise)
    {
        await _context.Exercises.InsertOneAsync(exercise);
    }

    public async Task SaveAsync()
    {
        await Task.CompletedTask;
    }

    public async Task SaveResultAsync(ExerciseResult result)
    {
        await _context.ExerciseResults.InsertOneAsync(result);
    }

    public async Task<ExerciseResult?> GetResultByIdAsync(Guid id)
    {
        return await _context.ExerciseResults
            .Find(r => r.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<ExerciseResult>> GetResultsByDateRangeAsync(DateTime? fromDate, DateTime? toDate, ExerciseType? typeFilter)
    {
        var filter = Builders<ExerciseResult>.Filter.Empty;

        if (fromDate.HasValue)
            filter &= Builders<ExerciseResult>.Filter.Gte(r => r.CompletedAt, fromDate);

        if (toDate.HasValue)
            filter &= Builders<ExerciseResult>.Filter.Lte(r => r.CompletedAt, toDate);

        if (typeFilter.HasValue)
            filter &= Builders<ExerciseResult>.Filter.Eq(r => r.Exercise.Type, typeFilter);

        return await _context.ExerciseResults
            .Find(filter)
            .SortByDescending(r => r.CompletedAt)
            .ToListAsync();
    }
}