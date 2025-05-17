using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Exceptions;
using GermanLearning.Domain.Rules;

namespace GermanLearning.Domain.Entities;

public class Word : EntityBase 
{
    public string GermanText { get; private set; }
    public string EnglishTranslation { get; private set; }
    public string SpanishTranslation { get; private set; }
    public WordType Type { get; private set; }
    public Gender? Gender { get; private set; }
    public string? Topic { get; private set; }
    public List<string> ExampleSentences { get; private set; }
    public List<string> Synonyms { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Word() { } // For EF Core

    public Word(
        string germanText,
        string englishTranslation,
        string spanishTranslation,
        WordType type,
        Gender? gender = null,
        string? topic = null,
        List<string>? exampleSentences = null,
        List<string>? synonyms = null)
    {
        Id = Guid.NewGuid();
        GermanText = germanText;
        EnglishTranslation = englishTranslation;
        SpanishTranslation = spanishTranslation;
        Type = type;
        Gender = type == WordType.Noun ? gender : null;
        Topic = topic;
        ExampleSentences = exampleSentences ?? new List<string>();
        Synonyms = synonyms ?? new List<string>();
        CreatedAt = DateTime.UtcNow;

        Validate();
    }

    public void Update(
        string germanText,
        string englishTranslation,
        string spanishTranslation,
        WordType type,
        Gender? gender = null,
        string? topic = null,
        List<string>? exampleSentences = null,
        List<string>? synonyms = null)
    {
        GermanText = germanText;
        EnglishTranslation = englishTranslation;
        SpanishTranslation = spanishTranslation;
        Type = type;
        Gender = type == WordType.Noun ? gender : null;
        Topic = topic;
        ExampleSentences = exampleSentences ?? new List<string>();
        Synonyms = synonyms ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;

        Validate();
    }

    private void Validate()
    {
        // Validate using business rules
        CheckRule(new TranslationNotEmptyRule(GermanText, "German"));
        CheckRule(new TranslationNotEmptyRule(EnglishTranslation, "English"));
        CheckRule(new TranslationNotEmptyRule(SpanishTranslation, "Spanish"));
        CheckRule(new NounMustHaveGenderRule(Type, Gender));

        // Validate collections
        if (ExampleSentences.Any(s => string.IsNullOrWhiteSpace(s)))
            throw new DomainValidationException("Example sentences cannot contain empty strings");

        if (Synonyms.Any(s => string.IsNullOrWhiteSpace(s)))
            throw new DomainValidationException("Synonyms cannot contain empty strings");
    }
}