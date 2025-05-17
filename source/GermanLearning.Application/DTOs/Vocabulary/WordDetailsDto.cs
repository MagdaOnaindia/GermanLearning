using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.DTOs.Vocabulary;

public record WordDetailsDto(
    Guid Id,
    string GermanText,
    string EnglishTranslation,
    string SpanishTranslation,
    WordType Type,
    Gender? Gender,
    string? Topic,
    List<string> ExampleSentences,
    List<string> Synonyms,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<WordDto> RelatedWords);