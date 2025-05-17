using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Exceptions;
using System.Reflection;
using Xunit;

namespace GermanLearning.Domain.Tests.Entities;

public class WordTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesWord()
    {
        // Arrange & Act
        var word = new Word(
            "Haus", "house", "casa",
            WordType.Noun, Gender.Neutral);

        // Assert
        Assert.Equal("Haus", word.GermanText);
        Assert.Equal(Gender.Neutral, word.Gender);
    }

    [Theory]
    [InlineData(null, "house", "casa")]
    [InlineData("", "house", "casa")]
    [InlineData("Haus", null, "casa")]
    [InlineData("Haus", "house", null)]
    public void Constructor_WithMissingTranslations_ThrowsException(
        string german, string english, string spanish)
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainValidationException>(() =>
            new Word(german, english, spanish, WordType.Noun, Gender.Neutral));
    }

    [Fact]
    public void Constructor_ForNounWithoutGender_ThrowsException()
    {
        Assert.Throws<DomainValidationException>(() =>
            new Word("Haus", "house", "casa", WordType.Noun, null));
    }

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        // Arrange
        var word = new Word("Haus", "house", "casa",
            WordType.Noun, Gender.Neutral);

        // Act
        word.Update("Auto", "car", "coche",
            WordType.Noun, Gender.Neutral);

        // Assert
        Assert.Equal("Auto", word.GermanText);
        Assert.NotNull(word.UpdatedAt);
    }
}