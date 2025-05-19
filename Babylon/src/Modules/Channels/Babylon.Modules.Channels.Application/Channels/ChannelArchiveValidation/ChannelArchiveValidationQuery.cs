using Babylon.Common.Application.Messaging;

namespace Babylon.Modules.Channels.Application.Channels.ChannelArchiveValidation;
public record ChannelArchiveValidationQuery(Guid ChannelId) : IQuery<bool>;
