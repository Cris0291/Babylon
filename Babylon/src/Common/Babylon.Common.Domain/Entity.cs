namespace Babylon.Common.Domain;
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity() { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public void RaiseEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
