using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Exceptions;

namespace GermanLearning.Domain.Entities;

public class Exercise : EntityBase
{
    public ExerciseType Type { get; private set; }
    public List<Word> Words { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public DateTime GeneratedAt { get; private set; }

    private Exercise() { } // For EF Core

    public Exercise(
        ExerciseType type,
        List<Word> words,
        DifficultyLevel difficulty)
    {
        if (words == null || words.Count == 0)
            throw new DomainValidationException("Exercise must contain words");

        Type = type;
        Words = words;
        Difficulty = difficulty;
        GeneratedAt = DateTime.UtcNow;

        Validate();
    }

    private void Validate()
    {
        switch (Type)
        {
            case ExerciseType.TranslationDEtoES:
            case ExerciseType.TranslationEStoDE:
                if (Words.Count < 1)
                    throw new DomainValidationException("Translation exercises need at least 1 word");
                break;

            case ExerciseType.VerbConjugation:
                if (Words.Any(w => w.Type != WordType.Verb))
                    throw new DomainValidationException("Verb conjugation exercises can only contain verbs");
                break;

            case ExerciseType.NounGender:
                if (Words.Any(w => w.Type != WordType.Noun))
                    throw new DomainValidationException("Noun gender exercises can only contain nouns");
                break;
        }
    }
}