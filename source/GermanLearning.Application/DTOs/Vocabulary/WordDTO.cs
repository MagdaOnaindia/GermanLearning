using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.DTO.Vocabulary;

public class WordDto
{
    public Guid Id { get; set; }
    public string GermanText { get; set; } = string.Empty;
    public List<string> EnglishTranslation { get; set; } = new();
    public List<string> SpanishTranslation { get; set; } = new();
    public WordType Type { get; set; }
    public string TypeString => Type.ToString(); // For display
    public Gender? Gender { get; set; }
    public string? GenderString => Gender?.ToString(); // For display
    public List<string> TopicNames { get; set; } = new(); // CHANGED from string? Topic
    public List<string> ExampleSentences { get; set; } = new();
    public List<string> Synonyms { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}