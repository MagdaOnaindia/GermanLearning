using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.SQLInfrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.SQLInfrastructure.Repositories;

public class WordRepository : IWordRepository
{
    private readonly AppDbContext _context;

    public WordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Word?> GetByIdAsync(Guid id)
    {
        return await _context.Words.FindAsync(id);
    }

    public async Task<List<Word>> GetAllAsync()
    {
        return await _context.Words.ToListAsync();
    }

    public async Task AddAsync(Word word)
    {
        await _context.Words.AddAsync(word);
    }

    public async Task UpdateAsync(Word word)
    {
        _context.Words.Update(word);
    }

    public async Task DeleteAsync(Guid id)
    {
        var word = await GetByIdAsync(id);
        if (word != null)
            _context.Words.Remove(word);
    }

    public async Task<List<Word>> GetByTypeAsync(WordType type)
    {
        return await _context.Words
            .Where(w => w.Type == type)
            .ToListAsync();
    }

    public async Task<List<Word>> GetByTopicAsync(string topic)
    {
        return await _context.Words
            .Where(w => w.Topic == topic)
            .ToListAsync();
    }

    public async Task<List<Word>> GetByTopicAndTypeAsync(string topic, WordType type)
    {
        return await _context.Words
            .Where(w => w.Topic == topic && w.Type == type)
            .ToListAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

