using GermanLearning.Application.DTOs.Exercises;
using GermanLearning.Application.Features.Exercises.Commands;
using GermanLearning.Application.Features.Exercises.Queries;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using FluentValidation;
using GermanLearning.Application.Mappings;

namespace GermanLearning.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWordRepository _wordRepository;
    private readonly IValidator<GenerateExerciseCommand> _generateExerciseValidator;
    private readonly IValidator<SaveExerciseResultCommand> _saveResultValidator;

    public ExerciseService(
        IExerciseRepository exerciseRepository,
        IWordRepository wordRepository,
        IValidator<GenerateExerciseCommand> generateExerciseValidator,
        IValidator<SaveExerciseResultCommand> saveResultValidator)
    {
        _exerciseRepository = exerciseRepository;
        _wordRepository = wordRepository;
        _generateExerciseValidator = generateExerciseValidator;
        _saveResultValidator = saveResultValidator;
    }

    public async Task<ExerciseDto> GenerateExerciseAsync(GenerateExerciseCommand command)
    {
        await _generateExerciseValidator.ValidateAndThrowAsync(command);

        List<Word> words = command.Type switch
        {
            ExerciseType.TranslationDEtoES or ExerciseType.TranslationEStoDE =>
                await GetWordsForTranslationExercise(command),
            ExerciseType.VerbConjugation => await _wordRepository.GetByTypeAsync(WordType.Verb),
            ExerciseType.NounGender => await _wordRepository.GetByTypeAsync(WordType.Noun),
            _ => throw new ArgumentOutOfRangeException(nameof(command.Type))
        };

        if (command.QuestionCount.HasValue)
        {
            words = words.Take(command.QuestionCount.Value).ToList();
        }

        var exercise = await _exerciseRepository.GenerateExerciseAsync(
            command.Type,
            command.Difficulty,
            words);

        await _exerciseRepository.SaveAsync();
        return exercise.ToDto();
    }

    private async Task<List<Word>> GetWordsForTranslationExercise(GenerateExerciseCommand command)
    {
        if (!string.IsNullOrEmpty(command.TopicFilter))
        {
            return command.WordTypeFilter.HasValue
                ? await _wordRepository.GetByTopicAndTypeAsync(command.TopicFilter, command.WordTypeFilter.Value)
                : await _wordRepository.GetByTopicAsync(command.TopicFilter);
        }

        return command.WordTypeFilter.HasValue
            ? await _wordRepository.GetByTypeAsync(command.WordTypeFilter.Value)
            : await _wordRepository.GetAllAsync();
    }

    public async Task<ExerciseResultDto> SaveExerciseResultAsync(SaveExerciseResultCommand command)
    {
        await _saveResultValidator.ValidateAndThrowAsync(command);

        var exerciseExists = await _exerciseRepository.GetByIdAsync(command.ExerciseId) != null;
        if (!exerciseExists)
        {
            throw new KeyNotFoundException("Exercise not found");
        }

        var result = new ExerciseResult(
            command.ExerciseId,
            command.CorrectAnswers,
            command.TotalQuestions);

        await _exerciseRepository.SaveResultAsync(result);
        await _exerciseRepository.SaveAsync();

        return result.ToDto();
    }

    public async Task<ExerciseDto> GetExerciseByIdAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id);
        return exercise?.ToDto() ?? throw new KeyNotFoundException("Exercise not found");
    }

    public async Task<List<ExerciseDto>> GetAllExercisesAsync()
    {
        var exercises = await _exerciseRepository.GetAllAsync();
        return exercises.Select(e => e.ToDto()).ToList();
    }

    public async Task<List<ExerciseResultDto>> GetExerciseHistoryAsync(GetExerciseHistoryQuery query)
    {
        var results = await _exerciseRepository.GetResultsByDateRangeAsync(
            query.FromDate,
            query.ToDate,
            query.TypeFilter);

        return results.Select(r => r.ToDto()).ToList();
    }

    public async Task<ExerciseResultDto> GetExerciseResultByIdAsync(Guid id)
    {
        var result = await _exerciseRepository.GetResultByIdAsync(id);
        return result?.ToDto() ?? throw new KeyNotFoundException("Exercise result not found");
    }
}