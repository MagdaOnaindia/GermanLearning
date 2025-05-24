// GermanLearning.PostgreInfrastructure/Repositories/TopicRepository.cs
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.PostgreInfrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GermanLearning.PostgreInfrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly AppDbContext _context;

    public TopicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Topic?> GetByIdAsync(Guid id)
    {
        return await _context.Topics.FindAsync(id);
    }

    public async Task<Topic?> GetByNameAsync(string name)
    {
        return await _context.Topics.FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }

    public async Task<List<Topic>> GetByNamesAsync(IEnumerable<string> names)
    {
        var lowerNames = names.Select(n => n.ToLower()).ToList();
        return await _context.Topics
            .Where(t => lowerNames.Contains(t.Name.ToLower()))
            .ToListAsync();
    }

    public async Task AddAsync(Topic topic)
    {
        await _context.Topics.AddAsync(topic);
    }

    public async Task AddRangeAsync(IEnumerable<Topic> topics)
    {
        await _context.Topics.AddRangeAsync(topics);
    }

    public async Task<List<Topic>> GetAllAsync()
    {
        return await _context.Topics.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic != null)
        {
            _context.Topics.Remove(topic);
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}