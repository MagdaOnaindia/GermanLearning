using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Application.Features.Vocabulary.Commands;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using FluentValidation;
using GermanLearning.Application.Mappings;

namespace GermanLearning.Application.Services;

public class WordService : IWordService
{
    private readonly IWordRepository _wordRepository;
    private readonly IValidator<AddWordCommand> _addWordValidator;
    private readonly IValidator<UpdateWordCommand> _updateWordValidator;

    public WordService(
        IWordRepository wordRepository,
        IValidator<AddWordCommand> addWordValidator,
        IValidator<UpdateWordCommand> updateWordValidator)
    {
        _wordRepository = wordRepository;
        _addWordValidator = addWordValidator;
        _updateWordValidator = updateWordValidator;
    }

    public async Task<WordDto> AddWordAsync(AddWordCommand command)
    {
        await _addWordValidator.ValidateAndThrowAsync(command);

        var word = new Word(
            command.GermanText,
            command.EnglishTranslation,
            command.SpanishTranslation,
            command.Type,
            command.Gender,
            command.Topic,
            command.ExampleSentences,
            command.Synonyms);

        await _wordRepository.AddAsync(word);
        await _wordRepository.SaveAsync();

        return word.ToDto();
    }

    public async Task UpdateWordAsync(UpdateWordCommand command)
    {
        await _updateWordValidator.ValidateAndThrowAsync(command);

        var word = await _wordRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException("Word not found");

        word.Update(
            command.GermanText,
            command.EnglishTranslation,
            command.SpanishTranslation,
            command.Type,
            command.Gender,
            command.Topic,
            command.ExampleSentences,
            command.Synonyms);

        await _wordRepository.UpdateAsync(word);
        await _wordRepository.SaveAsync();
    }

    public async Task DeleteWordAsync(Guid id)
    {
        var word = await _wordRepository.GetByIdAsync(id);
        if (word != null)
        {
            await _wordRepository.DeleteAsync(id);
            await _wordRepository.SaveAsync();
        }
    }

    public async Task<WordDto> GetWordByIdAsync(Guid id)
    {
        var word = await _wordRepository.GetByIdAsync(id);
        return word?.ToDto() ?? throw new KeyNotFoundException("Word not found");
    }

    public async Task<List<WordDto>> GetWordsByTypeAsync(WordType type)
    {
        var words = await _wordRepository.GetByTypeAsync(type);
        return words.Select(w => w.ToDto()).ToList();
    }

    public async Task<List<WordDto>> GetWordsByTopicAsync(string topic)
    {
        var words = await _wordRepository.GetByTopicAsync(topic);
        return words.Select(w => w.ToDto()).ToList();
    }

    public async Task<List<WordDto>> GetAllWordsAsync()
    {
        var words = await _wordRepository.GetAllAsync();
        return words.Select(w => w.ToDto()).ToList();
    }
}