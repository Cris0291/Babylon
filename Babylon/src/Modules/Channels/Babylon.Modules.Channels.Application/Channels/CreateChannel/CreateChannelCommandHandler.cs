using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;
using MediatR;

namespace Babylon.Modules.Channels.Application.Channels.CreateChannel;
internal sealed class CreateChannelCommandHandler(IChannelRepository channelRepository, IMemberRepository memberRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateChannelCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        bool existMember = await memberRepository.Exist(request.Id);
        if (!existMember)
        {
            throw new InvalidOperationException("Member was not found");
        }

        var channel = Channel.CreateChannel(request.ChannelName, request.IsPublicChannel, request.Id);

        await channelRepository.Insert(channel);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return channel.ChannelId;
    }
}
