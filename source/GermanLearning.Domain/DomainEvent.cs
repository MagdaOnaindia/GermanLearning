using System.ComponentModel.DataAnnotations.Schema;

namespace GermanLearning.Domain;

[NotMapped]
public abstract class DomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = false;
}