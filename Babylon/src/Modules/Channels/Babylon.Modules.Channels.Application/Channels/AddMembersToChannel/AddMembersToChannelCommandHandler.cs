using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
public class AddMembersToChannelCommandHandler(IChannelRepository channelRepository, IChannelMemberRepository channelMemberRepository ,IUnitOfWork unitOfWork) : ICommandHandler<AddMembersToChannelCommand>
{
    public async Task<Result> Handle(AddMembersToChannelCommand request, CancellationToken cancellationToken)
    {
        var membersList = new List<ChannelMember>();
        bool existChannel = await channelRepository.Exist(request.ChannelId);
        if (!existChannel)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        foreach (Guid member in request.MembersIds)
        {
            var memberChannel = ChannelMember.Create(request.ChannelId, member);
            membersList.Add(memberChannel);
        }

        await channelMemberRepository.AddChannelMembers(membersList);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
