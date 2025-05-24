using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using MediatR;

namespace GermanLearning.Application.Features.Vocabulary.Commands;

public class AddWordCommand
{
    public string GermanText { get; set; } = string.Empty;
    public List<string> EnglishTranslation { get; set; } = new();
    public List<string> SpanishTranslation { get; set; } = new();
    public WordType Type { get; set; }
    public Gender? Gender { get; set; }
    public List<string> TopicNames { get; set; } = new(); // CHANGED from string? Topic
    public List<string> ExampleSentences { get; set; } = new();
    public List<string> Synonyms { get; set; } = new();
}