namespace GermanLearning.Domain;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = false;
}