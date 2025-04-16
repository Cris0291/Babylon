
namespace Babylon.Common.Application.EventBus;
public abstract class IntegrationEvent : IIntegrationEvent
{
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.Now;
    }
    protected IntegrationEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }
    public Guid Id { get; }

    public DateTime OccurredOnUtc { get; }
}
