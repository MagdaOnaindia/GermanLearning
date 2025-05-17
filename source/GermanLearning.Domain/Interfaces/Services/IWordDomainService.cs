using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Domain.Interfaces.Services;

public interface IWordDomainService
{
    Task<Word> CreateWordAsync(
        string germanText,
        string englishTranslation,
        string spanishTranslation,
        WordType type,
        Gender? gender = null,
        string? topic = null,
        List<string>? exampleSentences = null,
        List<string>? synonyms = null);

    Task<Word> UpdateWordAsync(
        Guid wordId,
        string germanText,
        string englishTranslation,
        string spanishTranslation,
        WordType type,
        Gender? gender = null,
        string? topic = null,
        List<string>? exampleSentences = null,
        List<string>? synonyms = null);
}