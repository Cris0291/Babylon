using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using MediatR;

namespace Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
internal sealed class AddMemberToChannelDomainEventHandler(ISender sender) : DomainEventHandler<AddMemberToChannelDomainEvent>
{
    public override async Task Handle(AddMemberToChannelDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(new AddMembersToChannelCommand(domainEvent.ChannelId, domainEvent.UserId), cancellationToken);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException("Something unexpected happened. Member could not be added to requested channel");
        }
    }
}
