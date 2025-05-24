// GermanLearning.Domain/Entities/Word.cs
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Exceptions;
using GermanLearning.Domain.Rules;
using System.Collections.Generic; // Añade este using

namespace GermanLearning.Domain.Entities;

public class Word : EntityBase
{
    public string GermanText { get; private set; } // = default!;
    public List<string> EnglishTranslation { get; private set; } // = new List<string>();
    public List<string> SpanishTranslation { get; private set; } // = new List<string>();
    public WordType Type { get; private set; }
    public Gender? Gender { get; private set; }
    // public string? Topic { get; private set; } // <--- ELIMINAR ESTA LÍNEA

    public List<Topic> Topics { get; private set; } = new List<Topic>(); // <--- AÑADIR ESTA LÍNEA

    public List<string> ExampleSentences { get; private set; } // = new List<string>();
    public List<string> Synonyms { get; private set; } // = new List<string>();
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Constructor privado para EF Core (puede necesitar ajustes para la nueva colección)
    private Word()
    {
        // Inicializa las propiedades que eran non-nullable y no se inicializaban aquí
        GermanText = string.Empty; // o default!
        EnglishTranslation = new List<string>();
        SpanishTranslation = new List<string>();
        ExampleSentences = new List<string>();
        Synonyms = new List<string>();
        Topics = new List<Topic>(); // Importante inicializarla
    }

    public Word(
        string germanText,
        List<string> englishTranslation,
        List<string> spanishTranslation,
        WordType type,
        Gender? gender = null,
        List<Topic>? topics = null, // <--- MODIFICAR PARÁMETRO
        List<string>? exampleSentences = null,
        List<string>? synonyms = null)
    {
        Id = Guid.NewGuid(); // Asumiendo que se genera aquí
        GermanText = germanText;
        EnglishTranslation = englishTranslation;
        SpanishTranslation = spanishTranslation;
        Type = type;
        Gender = type == WordType.Noun ? gender : null;
        Topics = topics ?? new List<Topic>(); // <--- ASIGNAR LISTA DE TOPICS
        ExampleSentences = exampleSentences ?? new List<string>();
        Synonyms = synonyms ?? new List<string>();
        CreatedAt = DateTime.UtcNow;

        Validate();
    }

    public void Update(
        string germanText,
        List<string> englishTranslation,
        List<string> spanishTranslation,
        WordType type,
        Gender? gender = null,
        List<Topic>? topics = null, // <--- MODIFICAR PARÁMETRO
        List<string>? exampleSentences = null,
        List<string>? synonyms = null)
    {
        GermanText = germanText;
        EnglishTranslation = englishTranslation;
        SpanishTranslation = spanishTranslation;
        Type = type;
        Gender = type == WordType.Noun ? gender : null;
        Topics = topics ?? new List<Topic>(); // <--- ASIGNAR LISTA DE TOPICS
        ExampleSentences = exampleSentences ?? new List<string>();
        Synonyms = synonyms ?? new List<string>();
        UpdatedAt = DateTime.UtcNow;

        Validate();
    }

    private void Validate()
    {
        CheckRule(new TranslationNotEmptyRule(EnglishTranslation, "English"));
        CheckRule(new TranslationNotEmptyRule(SpanishTranslation, "Spanish"));
        CheckRule(new NounMustHaveGenderRule(Type, Gender));

        if (ExampleSentences.Any(s => string.IsNullOrWhiteSpace(s)))
            throw new DomainValidationException("Example sentences cannot contain empty strings");

        if (Synonyms.Any(s => string.IsNullOrWhiteSpace(s)))
            throw new DomainValidationException("Synonyms cannot contain empty strings");
    }
}