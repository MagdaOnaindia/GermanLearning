using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.Features.Vocabulary.Commands;

public class UpdateWordCommand
{
    public Guid Id { get; set; }
    public string GermanText { get; set; } = string.Empty;
    public List<string> EnglishTranslation { get; set; } = new();
    public List<string> SpanishTranslation { get; set; } = new();
    public WordType Type { get; set; }
    public Gender? Gender { get; set; }
    public List<string> TopicNames { get; set; } = new(); // CHANGED from string? Topic
    public List<string> ExampleSentences { get; set; } = new();
    public List<string> Synonyms { get; set; } = new();
}