// GermanLearning.Application/Services/WordService.cs
using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Application.Features.Vocabulary.Commands;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces; // Make sure this is IWordRepository
using FluentValidation;
using GermanLearning.Application.Mappings; // For ToDto()
using System.Linq; // For LINQ methods
using System.Collections.Generic; // For List
using System.Threading.Tasks; // For Task

namespace GermanLearning.Application.Services;

public class WordService : IWordService
{
    private readonly IWordRepository _wordRepository;
    private readonly ITopicRepository _topicRepository; // <<< ADD THIS
    private readonly IValidator<AddWordCommand> _addWordValidator;
    private readonly IValidator<UpdateWordCommand> _updateWordValidator;

    public WordService(
        IWordRepository wordRepository,
        ITopicRepository topicRepository, // <<< ADD THIS
        IValidator<AddWordCommand> addWordValidator,
        IValidator<UpdateWordCommand> updateWordValidator)
    {
        _wordRepository = wordRepository;
        _topicRepository = topicRepository; // <<< ADD THIS
        _addWordValidator = addWordValidator;
        _updateWordValidator = updateWordValidator;
    }

    private async Task<List<Topic>> GetOrCreateTopicsAsync(List<string> topicNames)
    {
        if (topicNames == null || !topicNames.Any())
        {
            return new List<Topic>();
        }

        var existingTopics = await _topicRepository.GetByNamesAsync(topicNames);
        var foundNames = existingTopics.Select(t => t.Name).ToList();
        var newTopicNames = topicNames.Except(foundNames, StringComparer.OrdinalIgnoreCase).ToList();

        var newTopics = new List<Topic>();
        foreach (var name in newTopicNames)
        {
            // Basic validation for topic name could be here or in Topic entity
            if (string.IsNullOrWhiteSpace(name)) continue;

            var newTopic = new Topic(name.Trim()); // Assuming Topic constructor takes a name
            newTopics.Add(newTopic);
            // If TopicRepository handles AddAsync and SaveAsync separately
            // await _topicRepository.AddAsync(newTopic);
        }

        // If new topics were created and TopicRepository doesn't save on AddAsync
        if (newTopics.Any() ) // Avoid double save if using same context
        {
            // This part is tricky if TopicRepository and WordRepository share a Unit of Work (DbContext)
            // It's better if ITopicRepository.AddAsync also persists or you have a UnitOfWork pattern.
            // For now, let's assume AddRangeAsync and SaveAsync are available or Topic constructor is enough.
            // A cleaner approach uses a Unit of Work pattern.
            await _topicRepository.AddRangeAsync(newTopics); // Assuming this method exists
            // await _topicRepository.SaveAsync(); // Or save changes later with word
        }

        return existingTopics.Concat(newTopics).ToList();
    }

    public async Task<WordDto> AddWordAsync(AddWordCommand command)
    {
        await _addWordValidator.ValidateAndThrowAsync(command);

        // Handle Topics
        List<Topic> topicsForWord = await GetOrCreateTopicsAsync(command.TopicNames);

        var word = new Word(
            command.GermanText,
            command.EnglishTranslation,
            command.SpanishTranslation,
            command.Type,
            command.Gender,
            topicsForWord, // Pass the list of Topic entities
            command.ExampleSentences,
            command.Synonyms);

        await _wordRepository.AddAsync(word);
        await _wordRepository.SaveAsync(); // This should save the word and its relationships

        return word.ToDto();
    }

    public async Task UpdateWordAsync(UpdateWordCommand command)
    {
        await _updateWordValidator.ValidateAndThrowAsync(command);

        var word = await _wordRepository.GetByIdAsync(command.Id, includeTopics: true) // Need to load existing topics
            ?? throw new KeyNotFoundException("Word not found");

        // Handle Topics for update
        List<Topic> updatedTopicsForWord = await GetOrCreateTopicsAsync(command.TopicNames);

        // Clear existing topics and add new ones
        // (More sophisticated logic might be needed for performance with many topics,
        // but for typical scenarios, clearing and re-adding is straightforward)
        // This requires word.Topics to be a modifiable collection or have methods to manage it.
        // Ensure Word entity's Topics collection can be cleared and added to.
        // If Word.Topics is `private set`, you need methods in Word entity like ClearTopics(), AddTopic().
        // For simplicity, let's assume we can directly modify it (EF Core tracks changes).

        // EF Core needs to know which topics are removed and which are added.
        // The simplest way if the `Topics` collection in the `Word` entity is tracked:
        word.Topics.Clear(); // This will mark existing relationships for deletion
        foreach (var topic in updatedTopicsForWord)
        {
            word.Topics.Add(topic); // This will mark new relationships for addition
        }
        // Alternatively, you'd have methods in your Word entity: word.UpdateTopics(updatedTopicsForWord);

        word.Update( // This method in Word entity needs to accept List<Topic>
            command.GermanText,
            command.EnglishTranslation,
            command.SpanishTranslation,
            command.Type,
            command.Gender,
            updatedTopicsForWord, // Ensure word.Update can take List<Topic> or handle topics separately
            command.ExampleSentences,
            command.Synonyms);

        // If word.Update doesn't handle topics, assign them directly IF ALLOWED by encapsulation:
        // word.Topics = updatedTopicsForWord; // This replaces the collection. EF Core will diff.

        await _wordRepository.UpdateAsync(word); // EF Core will detect changes to the Word and its Topics collection
        await _wordRepository.SaveAsync();
    }

    public async Task DeleteWordAsync(Guid id)
    {
        // Current implementation is fine, deleting a word will also remove its
        // entries in the WordTopic join table due to cascading deletes (if configured)
        // or EF Core managing the relationship.
        var word = await _wordRepository.GetByIdAsync(id);
        if (word != null)
        {
            await _wordRepository.DeleteAsync(id);
            await _wordRepository.SaveAsync();
        }
    }

    public async Task<WordDto> GetWordByIdAsync(Guid id)
    {
        // Need to ensure Topics are loaded if WordDto expects them
        var word = await _wordRepository.GetByIdAsync(id, includeTopics: true);
        return word?.ToDto() ?? throw new KeyNotFoundException("Word not found");
    }

    public async Task<List<WordDto>> GetWordsByTypeAsync(WordType type)
    {
        // Need to ensure Topics are loaded for each word
        var words = await _wordRepository.GetByTypeAsync(type, includeTopics: true);
        return words.Select(w => w.ToDto()).ToList();
    }

    // GetWordsByTopicAsync needs a new implementation logic
    public async Task<List<WordDto>> GetWordsByTopicAsync(string topicName)
    {
        // Ensure topics are loaded for the DTO conversion
        var words = await _wordRepository.GetByTopicNameAsync(topicName, includeTopics: true);
        return words.Select(w => w.ToDto()).ToList();
    }

    public async Task<List<WordDto>> GetAllWordsAsync()
    {
        // Need to ensure Topics are loaded for each word
        var words = await _wordRepository.GetAllAsync(includeTopics: true);
        return words.Select(w => w.ToDto()).ToList();
    }
}