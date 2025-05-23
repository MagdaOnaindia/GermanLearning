using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.DTO.Vocabulary;

public record WordDto(
    Guid Id,
    string GermanText,
    List<string> EnglishTranslation,
    List<string> SpanishTranslation,
    WordType Type,
    Gender? Gender,
    string? Topic,
    List<string> ExampleSentences,
    List<string> Synonyms,
    DateTime CreatedAt,
    DateTime? UpdatedAt);