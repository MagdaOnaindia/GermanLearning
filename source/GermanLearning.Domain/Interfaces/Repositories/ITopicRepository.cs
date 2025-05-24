// GermanLearning.Domain/Repositories/Interfaces/ITopicRepository.cs
using GermanLearning.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GermanLearning.Domain.Repositories.Interfaces;

public interface ITopicRepository
{
    Task<Topic?> GetByIdAsync(Guid id);
    Task<Topic?> GetByNameAsync(string name);
    Task<List<Topic>> GetByNamesAsync(IEnumerable<string> names);
    Task AddAsync(Topic topic);
    Task AddRangeAsync(IEnumerable<Topic> topics); // For adding multiple new topics
    Task<List<Topic>> GetAllAsync();
    Task DeleteAsync(Guid id);
    Task SaveAsync(); // Or rely on a Unit of Work
}

