// GermanLearning.Domain/Repositories/Interfaces/IWordRepository.cs
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

public interface IWordRepository
{
    Task<Word?> GetByIdAsync(Guid id, bool includeTopics = false); // Add includeTopics
    Task<List<Word>> GetAllAsync(bool includeTopics = false); // Add includeTopics
    Task AddAsync(Word word);
    Task UpdateAsync(Word word);
    Task DeleteAsync(Guid id);
    Task<List<Word>> GetByTypeAsync(WordType type, bool includeTopics = false); // Add includeTopics
    Task<List<Word>> GetByTopicNameAsync(string topicName, bool includeTopics = false); // New method name
    Task<List<Word>> GetByTopicNameAndTypeAsync(string topicName, WordType type, bool includeTopics = false); // New or ensure this exists

    // Task<List<Word>> GetByTopicAndTypeAsync(string topic, WordType type); // Review this
    Task SaveAsync();
}