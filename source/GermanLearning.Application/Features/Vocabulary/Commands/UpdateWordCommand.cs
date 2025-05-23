using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.Features.Vocabulary.Commands;

public record UpdateWordCommand(
    Guid Id,
    string GermanText,
    List<string> EnglishTranslation,
    List<string> SpanishTranslation,
    WordType Type,
    Gender? Gender,
    string Topic,
    List<string> ExampleSentences,
    List<string> Synonyms);