using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Domain.Repositories.Interfaces;

public interface IWordRepository
{
    Task<Word?> GetByIdAsync(Guid id);
    Task<List<Word>> GetAllAsync();
    Task AddAsync(Word word);
    Task UpdateAsync(Word word);
    Task DeleteAsync(Guid id);
    Task<List<Word>> GetByTypeAsync(WordType type);
    Task<List<Word>> GetByTopicAsync(string topic);
    Task<List<Word>> GetByTopicAndTypeAsync(string topic, WordType type);
    Task SaveAsync(); // Instead of SaveChangesAsync
}