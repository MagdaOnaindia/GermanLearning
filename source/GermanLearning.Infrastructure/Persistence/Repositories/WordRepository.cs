using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Repositories.Interfaces;
using MongoDB.Driver;
using GermanLearning.Infrastructure.Persistence.Contexts;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Infrastructure.Persistence.Repositories;

public class WordRepository : IWordRepository
{
    private readonly MongoDbContext _context;

    public WordRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<Word?> GetByIdAsync(Guid id)
    {
        return await _context.Words
            .Find(w => w.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Word>> GetAllAsync()
    {
        return await _context.Words
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task AddAsync(Word word)
    {
        await _context.Words.InsertOneAsync(word);
    }

    public async Task UpdateAsync(Word word)
    {
        await _context.Words
            .ReplaceOneAsync(w => w.Id == word.Id, word);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Words
            .DeleteOneAsync(w => w.Id == id);
    }

    public async Task<List<Word>> GetByTypeAsync(WordType type)
    {
        return await _context.Words
            .Find(w => w.Type == type)
            .ToListAsync();
    }

    public async Task<List<Word>> GetByTopicAsync(string topic)
    {
        return await _context.Words
            .Find(w => w.Topic == topic)
            .ToListAsync();
    }

    public async Task<List<Word>> GetByTopicAndTypeAsync(string topic, WordType type)
    {
        return await _context.Words
            .Find(w => w.Topic == topic && w.Type == type)
            .ToListAsync();
    }

    public async Task SaveAsync()
    {
        // MongoDB doesn't need explicit SaveChanges like EF Core
        await Task.CompletedTask;
    }
}