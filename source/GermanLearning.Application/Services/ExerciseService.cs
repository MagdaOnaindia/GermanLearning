// GermanLearning.Application/Services/ExerciseService.cs
using GermanLearning.Application.DTOs.Exercises;
using GermanLearning.Application.Features.Exercises.Commands;
using GermanLearning.Application.Features.Exercises.Queries;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using FluentValidation;
using GermanLearning.Application.Mappings;
using System; // For ArgumentOutOfRangeException
using System.Collections.Generic; // For List
using System.Linq; // For Take
using System.Threading.Tasks; // For Task

namespace GermanLearning.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWordRepository _wordRepository;
    // ITopicRepository is not directly needed for fetching words for exercise generation anymore
    // private readonly ITopicRepository _topicRepository; 
    private readonly IValidator<GenerateExerciseCommand> _generateExerciseValidator;
    private readonly IValidator<SaveExerciseResultCommand> _saveResultValidator;

    public ExerciseService(
        IExerciseRepository exerciseRepository,
        IWordRepository wordRepository,
        // ITopicRepository topicRepository, // Can remove if not used elsewhere in this service
        IValidator<GenerateExerciseCommand> generateExerciseValidator,
        IValidator<SaveExerciseResultCommand> saveResultValidator)
    {
        _exerciseRepository = exerciseRepository;
        _wordRepository = wordRepository;
        // _topicRepository = topicRepository;
        _generateExerciseValidator = generateExerciseValidator;
        _saveResultValidator = saveResultValidator;
    }

    public async Task<ExerciseDto> GenerateExerciseAsync(GenerateExerciseCommand command)
    {
        await _generateExerciseValidator.ValidateAndThrowAsync(command);

        List<Word> candidateWords; // Renamed for clarity

        // For exercise generation, we typically don't need to load the full Topic objects onto each Word.
        // The filtering happens in the repository. So, includeTopics can be false.
        const bool includeTopicsOnWordForExercise = false;

        switch (command.Type)
        {
            case ExerciseType.TranslationDEtoES:
            case ExerciseType.TranslationEStoDE:
                candidateWords = await GetWordsForTranslationExercise(command, includeTopicsOnWordForExercise);
                break;
            case ExerciseType.VerbConjugation:
                candidateWords = await _wordRepository.GetByTypeAsync(WordType.Verb, includeTopicsOnWordForExercise);
                break;
            case ExerciseType.NounGender:
                candidateWords = await _wordRepository.GetByTypeAsync(WordType.Noun, includeTopicsOnWordForExercise);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command.Type), "Unsupported exercise type.");
        }

        if (!candidateWords.Any())
        {
            // Or throw a custom exception like NoWordsFoundForCriteriaException
            // For now, we can let the Exercise constructor handle empty words list if it does.
            // Or, return an "empty" ExerciseDto or a specific error DTO.
            // Let's assume Exercise constructor throws if words are empty as per your domain entity.
        }

        // Shuffle and select words
        var random = new Random();
        var selectedWords = candidateWords.OrderBy(x => random.Next()).ToList(); // Shuffle

        if (command.QuestionCount.HasValue && command.QuestionCount.Value > 0)
        {
            selectedWords = selectedWords.Take(command.QuestionCount.Value).ToList();
        }
        // If after shuffling and taking, selectedWords is empty, the Exercise constructor will likely throw.
        // You might want to add a check here:
        if (!selectedWords.Any() && candidateWords.Any()) // if QuestionCount was too small or 0
        {
            // Handle this case - e.g. throw, or adjust QuestionCount
            // For now, we proceed, Exercise constructor should validate.
        }


        var exercise = new Exercise( // Using Domain Entity constructor
            command.Type,
            selectedWords,
            command.Difficulty
            );
        // The Exercise entity constructor should validate if selectedWords is empty.

        // Note: _exerciseRepository.GenerateExerciseAsync was removed, assuming
        // the Exercise entity constructor is sufficient and the repository just adds it.
        // If _exerciseRepository.GenerateExerciseAsync had more complex logic, adapt accordingly.
        await _exerciseRepository.AddAsync(exercise); // Assuming AddAsync for new exercises
        await _exerciseRepository.SaveAsync();

        return exercise.ToDto(); // Ensure Exercise.ToDto mapping exists and works
    }

    // Updated to use IWordRepository
    private async Task<List<Word>> GetWordsForTranslationExercise(GenerateExerciseCommand command, bool includeTopicsOnWord)
    {
        if (!string.IsNullOrWhiteSpace(command.TopicFilter))
        {
            return command.WordTypeFilter.HasValue
                ? await _wordRepository.GetByTopicNameAndTypeAsync(command.TopicFilter, command.WordTypeFilter.Value, includeTopicsOnWord)
                : await _wordRepository.GetByTopicNameAsync(command.TopicFilter, includeTopicsOnWord);
        }
        else // No topic filter
        {
            return command.WordTypeFilter.HasValue
                ? await _wordRepository.GetByTypeAsync(command.WordTypeFilter.Value, includeTopicsOnWord)
                : await _wordRepository.GetAllAsync(includeTopicsOnWord);
        }
    }

    // ... SaveExerciseResultAsync and other methods remain largely the same ...
    // Ensure that GetByIdAsync in IExerciseRepository also loads Words for the DTO
    public async Task<ExerciseDto> GetExerciseByIdAsync(Guid id)
    {
        // Assuming ExerciseRepository.GetByIdAsync loads associated words
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        return exercise?.ToDto() ?? throw new KeyNotFoundException($"Exercise with id {id} not found.");
    }

    public async Task<List<ExerciseDto>> GetAllExercisesAsync()
    {
        // Assuming ExerciseRepository.GetAllAsync loads associated words for each exercise
        var exercises = await _exerciseRepository.GetAllAsync();
        return exercises.Select(e => e.ToDto()).ToList();
    }

    Task<ExerciseResultDto> IExerciseService.SaveExerciseResultAsync(SaveExerciseResultCommand command)
    {
        throw new NotImplementedException();
    }

    Task<List<ExerciseResultDto>> IExerciseService.GetExerciseHistoryAsync(GetExerciseHistoryQuery query)
    {
        throw new NotImplementedException();
    }
    // ... rest of the service
}