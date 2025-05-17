namespace GermanLearning.Domain.Repositories.Services;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync(IEnumerable<EntityBase> entities);
}