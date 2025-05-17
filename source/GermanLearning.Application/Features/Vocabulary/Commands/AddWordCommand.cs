using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Enums;
using MediatR;

namespace GermanLearning.Application.Features.Vocabulary.Commands;

public record AddWordCommand(
    string GermanText,
    string EnglishTranslation,
    string SpanishTranslation,
    WordType Type,
    Gender? Gender,
    string? Topic,
    List<string>? ExampleSentences,
    List<string>? Synonyms) : IRequest<WordDto>;