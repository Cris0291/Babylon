using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
public sealed record GetValidThreadChannelQuery(Guid Id, Guid ThreadId) : IQuery<bool>;
