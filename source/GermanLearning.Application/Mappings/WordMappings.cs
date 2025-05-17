using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Entities;

namespace GermanLearning.Application.Mappings;

public static class WordMappings
{
    public static WordDto ToDto(this Word word)
    {
        return new WordDto(
            word.Id,
            word.GermanText,
            word.EnglishTranslation,
            word.SpanishTranslation,
            word.Type,
            word.Gender,
            word.Topic,
            word.ExampleSentences,
            word.Synonyms,
            word.CreatedAt,
            word.UpdatedAt);
    }

    //public static Word ToEntity(this WordDto dto)
    //{
    //    return new Word(
    //        dto.GermanText,
    //        dto.EnglishTranslation,
    //        dto.SpanishTranslation,
    //        dto.Type,
    //        dto.Gender,
    //        dto.Topic,
    //        dto.ExampleSentences,
    //        dto.Synonyms)
    //    {
    //        Id = dto.Id,
    //        CreatedAt = dto.CreatedAt,
    //        UpdatedAt = dto.UpdatedAt
    //    };
    //}
}