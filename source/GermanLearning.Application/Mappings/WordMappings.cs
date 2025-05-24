// GermanLearning.Application/Mappings/WordMappings.cs (or similar file)
using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Entities;
using System.Linq; // Add this

namespace GermanLearning.Application.Mappings;

public static class WordMappings
{
    public static WordDto ToDto(this Word word)
    {
        if (word == null) return null!; // Or throw, or return new WordDto()

        return new WordDto
        {
            Id = word.Id,
            GermanText = word.GermanText,
            EnglishTranslation = word.EnglishTranslation,
            SpanishTranslation = word.SpanishTranslation,
            Type = word.Type,
            Gender = word.Gender,
            // Map the list of Topic entities to a list of topic names
            TopicNames = word.Topics?.Select(t => t.Name).ToList() ?? new List<string>(), // CHANGED
            ExampleSentences = word.ExampleSentences,
            Synonyms = word.Synonyms,
            CreatedAt = word.CreatedAt,
            UpdatedAt = word.UpdatedAt
        };
    }
}