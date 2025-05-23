using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.PostgreInfrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GermanLearning.PostgreInfrastructure.Repositories;

    public class ExerciseRepository : IExerciseRepository
    {
        private readonly AppDbContext _context;

        public ExerciseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Exercise?> GetByIdAsync(Guid id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        public async Task<List<Exercise>> GetAllAsync()
        {
            return await _context.Exercises.ToListAsync();
        }

        public async Task<Exercise> GenerateExerciseAsync(ExerciseType type, DifficultyLevel difficulty, List<Word> words)
        {
            var exercise = new Exercise(type, words, difficulty);
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        public async Task AddAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);
        }

        public async Task SaveResultAsync(ExerciseResult result)
        {
            await _context.ExerciseResults.AddAsync(result);
        }

        public async Task<ExerciseResult?> GetResultByIdAsync(Guid id)
        {
            return await _context.ExerciseResults.FindAsync(id);
        }

        public async Task<List<ExerciseResult>> GetResultsByDateRangeAsync(DateTime? fromDate, DateTime? toDate, ExerciseType? typeFilter)
        {
            var query = _context.ExerciseResults.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(r => r.CompletedAt >= fromDate);

            if (toDate.HasValue)
                query = query.Where(r => r.CompletedAt <= toDate);

            if (typeFilter.HasValue)
                query = query.Where(r => r.Exercise.Type == typeFilter);

            return await query
                .OrderByDescending(r => r.CompletedAt)
                .ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


