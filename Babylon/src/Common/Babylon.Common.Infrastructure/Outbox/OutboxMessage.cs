﻿using Babylon.Common.Domain;

namespace Babylon.Common.Infrastructure.Outbox;
public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Content { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; init; }
    public Error? Error { get; init; }
}
