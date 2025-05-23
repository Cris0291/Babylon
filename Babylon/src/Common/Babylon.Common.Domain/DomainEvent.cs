﻿namespace Babylon.Common.Domain;
public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.Now;
    }
    protected DomainEvent(Guid id, DateTime occurredOnUtc)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
    }
    public Guid Id { get; }

    public DateTime OccurredOnUtc
    {
        get;
    }
}
