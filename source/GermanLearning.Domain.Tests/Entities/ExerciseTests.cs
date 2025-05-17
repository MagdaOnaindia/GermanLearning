using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Exceptions;
using Xunit;

namespace GermanLearning.Domain.Tests.Entities;

public class ExerciseTests
{
    [Fact]
    public void Constructor_WithValidWords_CreatesExercise()
    {
        // Arrange
        var words = new List<Word>
        {
            new Word("gehen", "to go", "ir", WordType.Verb),
            new Word("kommen", "to come", "venir", WordType.Verb)
        };

        // Act
        var exercise = new Exercise(
            ExerciseType.VerbConjugation,
            words,
            DifficultyLevel.Beginner);

        // Assert
        Assert.Equal(2, exercise.Words.Count);
        Assert.Equal(ExerciseType.VerbConjugation, exercise.Type);
    }

    [Fact]
    public void Constructor_WithEmptyWordList_ThrowsException()
    {
        // Arrange
        var emptyWords = new List<Word>();

        // Act & Assert
        Assert.Throws<DomainValidationException>(() =>
            new Exercise(
                ExerciseType.TranslationDEtoES,
                emptyWords,
                DifficultyLevel.Beginner));
    }

    [Theory]
    [InlineData(ExerciseType.VerbConjugation, WordType.Noun)]
    [InlineData(ExerciseType.NounGender, WordType.Verb)]
    public void Constructor_WithInvalidWordTypes_ThrowsException(
        ExerciseType exerciseType, WordType wordType)
    {
        // Arrange
        var invalidWords = new List<Word>
        {
            new Word("test", "test", "test", wordType)
        };

        // Act & Assert
        Assert.Throws<DomainValidationException>(() =>
            new Exercise(
                exerciseType,
                invalidWords,
                DifficultyLevel.Beginner));
    }

    [Fact]
    public void Constructor_ForTranslationExercise_AcceptsMixedWordTypes()
    {
        // Arrange
        var words = new List<Word>
        {
            new Word("Haus", "house", "casa", WordType.Noun, Gender.Neutral),
            new Word("gehen", "to go", "ir", WordType.Verb)
        };

        // Act
        var exercise = new Exercise(
            ExerciseType.TranslationDEtoES,
            words,
            DifficultyLevel.Intermediate);

        // Assert
        Assert.Equal(2, exercise.Words.Count);
    }
    [Fact]
    public void Constructor_ForNounGenderExercise_RequiresNouns()
    {
        var invalidWords = new List<Word>
    {
        new Word("gehen", "to go", "ir", WordType.Verb)
    };

        Assert.Throws<DomainValidationException>(() =>
            new Exercise(
                ExerciseType.NounGender,
                invalidWords,
                DifficultyLevel.Beginner));
    }

    [Fact]
    public void Constructor_SetsGeneratedAtToUtcNow()
    {
        var words = new List<Word>
    {
        new Word("Haus", "house", "casa", WordType.Noun, Gender.Neutral)
    };

        var beforeCreation = DateTime.UtcNow;
        var exercise = new Exercise(
            ExerciseType.TranslationDEtoES,
            words,
            DifficultyLevel.Beginner);
        var afterCreation = DateTime.UtcNow;

        Assert.InRange(exercise.GeneratedAt, beforeCreation, afterCreation);
    }

}