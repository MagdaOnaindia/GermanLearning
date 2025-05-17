using GermanLearning.Domain.ValueObjects;
using GermanLearning.Domain.Exceptions;
using Xunit;

namespace GermanLearning.Domain.Tests.ValueObjects;

public class TranslationTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesTranslation()
    {
        // Arrange & Act
        var translation = new Translation("Haus", "house", "casa");

        // Assert
        Assert.Equal("Haus", translation.German);
    }

    [Theory]
    [InlineData(null, "house", "casa")]
    [InlineData("", "house", "casa")]
    [InlineData("Haus", null, "casa")]
    public void Constructor_WithInvalidData_ThrowsException(
        string german, string english, string spanish)
    {
        Assert.Throws<DomainValidationException>(() =>
            new Translation(german, english, spanish));
    }
}