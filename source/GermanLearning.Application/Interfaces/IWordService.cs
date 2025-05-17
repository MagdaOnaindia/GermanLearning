using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Application.Features.Vocabulary.Commands;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.Interfaces;

public interface IWordService
{
    Task<WordDto> AddWordAsync(AddWordCommand command);
    Task UpdateWordAsync(UpdateWordCommand command);
    Task DeleteWordAsync(Guid id);
    Task<List<WordDto>> GetAllWordsAsync();
    Task<WordDto> GetWordByIdAsync(Guid id);
    Task<List<WordDto>> GetWordsByTypeAsync(WordType type);
    Task<List<WordDto>> GetWordsByTopicAsync(string topic);
}