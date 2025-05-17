using GermanLearning.Domain.Exceptions;

namespace GermanLearning.Domain.ValueObjects;

public record Translation
{
    public string German { get; }
    public string English { get; }
    public string Spanish { get; }

    public Translation(string german, string english, string spanish)
    {
        if (string.IsNullOrWhiteSpace(german))
            throw new DomainValidationException("German text cannot be empty");

        if (string.IsNullOrWhiteSpace(english))
            throw new DomainValidationException("English text cannot be empty");

        if (string.IsNullOrWhiteSpace(spanish))
            throw new DomainValidationException("Spanish text cannot be empty");

        German = german;
        English = english;
        Spanish = spanish;
    }
}