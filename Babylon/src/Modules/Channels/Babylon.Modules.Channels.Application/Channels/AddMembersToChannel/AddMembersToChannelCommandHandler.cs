using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
internal sealed class AddMembersToChannelCommandHandler(IChannelRepository channelRepository, IChannelMemberRepository channelMemberRepository, IMemberRepository memberRepository, IUnitOfWork unitOfWork) : ICommandHandler<AddMembersToChannelCommand>
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
            bool existMember = await memberRepository.Exist(member);
            if (existMember)
            {
                var memberChannel = ChannelMember.Create(request.ChannelId, member);
                membersList.Add(memberChannel);
            }
            else
            {
                throw new InvalidOperationException("Member was not found");
            }
        }

        await channelMemberRepository.AddChannelMembers(membersList);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
