using GermanLearning.Domain.Rules;
using Xunit;

namespace GermanLearning.Domain.Tests.Rules;

public class TranslationNotEmptyRuleTests
{
    [Theory]
    [InlineData(null, "German", true)]  // null text should break rule
    [InlineData("", "German", true)]    // empty string should break rule
    [InlineData("   ", "German", true)] // whitespace should break rule
    [InlineData("Haus", "German", false)] // valid text should not break rule
    public void IsBroken_VariousInputs_ReturnsCorrectResult(
        string translation, string language, bool expected)
    {
        // Arrange
        var rule = new TranslationNotEmptyRule(translation, language);

        // Act
        var result = rule.IsBroken();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Message_ContainsCorrectLanguage()
    {
        // Arrange
        var rule = new TranslationNotEmptyRule("", "Spanish");

        // Act
        var message = rule.Message;

        // Assert
        Assert.Contains("Spanish", message);
    }

    [Theory]
    [InlineData("Haus", "German", false)]
    [InlineData("house", "English", false)]
    [InlineData("casa", "Spanish", false)]
    public void IsBroken_WithValidTranslations_ReturnsFalse(
    string translation, string language, bool expected)
    {
        var rule = new TranslationNotEmptyRule(translation, language);
        Assert.Equal(expected, rule.IsBroken());
    }
}