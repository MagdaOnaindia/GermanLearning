using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Domain.Events;

public class WordCreatedEvent : DomainEvent
{
    public Guid WordId { get; }
    public string GermanText { get; }
    public WordType WordType { get; }

    public WordCreatedEvent(Word word)
    {
        WordId = word.Id;
        GermanText = word.GermanText;
        WordType = word.Type;
        OccurredOn = DateTime.UtcNow;
    }
}