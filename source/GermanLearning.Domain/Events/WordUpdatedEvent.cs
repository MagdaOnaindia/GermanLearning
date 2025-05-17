namespace GermanLearning.Domain.Events;

public class WordUpdatedEvent : DomainEvent
{
    public Guid WordId { get; }
    public string PreviousGermanText { get; }
    public string NewGermanText { get; }

    public WordUpdatedEvent(Guid wordId, string previousGermanText, string newGermanText)
    {
        WordId = wordId;
        PreviousGermanText = previousGermanText;
        NewGermanText = newGermanText;
    }
}