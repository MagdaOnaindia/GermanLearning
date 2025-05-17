using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Rules;
using Xunit;

namespace GermanLearning.Domain.Tests.Rules;

public class NounMustHaveGenderRuleTests
{
    [Theory]
    [InlineData(WordType.Noun, null, true)] // Broken
    [InlineData(WordType.Noun, Gender.Neutral, false)] // Valid
    [InlineData(WordType.Verb, null, false)] // Valid (not noun)
    public void IsBroken_VariousScenarios_ReturnsCorrectResult(
        WordType type, Gender? gender, bool expected)
    {
        // Arrange
        var rule = new NounMustHaveGenderRule(type, gender);

        // Act & Assert
        Assert.Equal(expected, rule.IsBroken());
    }
}