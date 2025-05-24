// GermanLearning.PostgreInfrastructure/Repositories/WordRepository.cs
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.PostgreInfrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GermanLearning.PostgreInfrastructure.Repositories;

public class WordRepository : IWordRepository
{
    private readonly AppDbContext _context;

    public WordRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<Word> IncludeRelated(IQueryable<Word> query, bool includeTopics)
    {
        if (includeTopics)
        {
            query = query.Include(w => w.Topics);
        }
        // You could add .Include for WordTypeEntityNavigation and GenderEntityNavigation here too if needed
        // query = query.Include(w => w.WordTypeEntityNavigation).Include(w => w.GenderEntityNavigation);
        return query;
    }

    public async Task<Word?> GetByIdAsync(Guid id, bool includeTopics = false)
    {
        IQueryable<Word> query = _context.Words;
        query = IncludeRelated(query, includeTopics);
        return await query.FirstOrDefaultAsync(w => w.Id == id); // Use FirstOrDefaultAsync with Id check
    }

    public async Task<List<Word>> GetAllAsync(bool includeTopics = false)
    {
        IQueryable<Word> query = _context.Words;
        query = IncludeRelated(query, includeTopics);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Word word)
    {
        // EF Core will handle inserting into Word and WordTopic join table
        await _context.Words.AddAsync(word);
    }

    public async Task UpdateAsync(Word word)
    {
        // EF Core change tracker will detect changes to Word and its Topics collection
        _context.Words.Update(word);
    }

    public async Task DeleteAsync(Guid id)
    {
        var word = await GetByIdAsync(id); // No need to include topics for delete
        if (word != null)
            _context.Words.Remove(word); // EF Core handles join table records
    }

    public async Task<List<Word>> GetByTypeAsync(WordType type, bool includeTopics = false)
    {
        IQueryable<Word> query = _context.Words.Where(w => w.Type == type);
        query = IncludeRelated(query, includeTopics);
        return await query.ToListAsync();
    }

    // Changed from GetByTopicAsync(string topic)
    public async Task<List<Word>> GetByTopicNameAsync(string topicName, bool includeTopics = false)
    {
        // Find words that have at least one topic with the given name.
        IQueryable<Word> query = _context.Words
            .Where(w => w.Topics.Any(t => t.Name.ToLower() == topicName.ToLower()));
        query = IncludeRelated(query, includeTopics);
        return await query.ToListAsync();
    }
    public async Task<List<Word>> GetByTopicNameAndTypeAsync(string topicName, WordType type, bool includeTopics = false)
    {
        IQueryable<Word> query = _context.Words
            .Where(w => w.Topics.Any(t => t.Name.ToLower() == topicName.ToLower()) && w.Type == type);
        query = IncludeRelated(query, includeTopics);
        return await query.ToListAsync();
    }
    public async Task<List<Word>> GetByTopicAndTypeAsync(string topicName, WordType type, bool includeTopics = false)
    {
        IQueryable<Word> query = _context.Words
            .Where(w => w.Topics.Any(t => t.Name.ToLower() == topicName.ToLower()) && w.Type == type);
        query = IncludeRelated(query, includeTopics);
        return await query.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}