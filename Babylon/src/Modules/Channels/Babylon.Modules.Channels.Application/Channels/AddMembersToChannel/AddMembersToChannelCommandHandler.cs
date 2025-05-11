using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;

namespace Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
internal sealed class AddMembersToChannelCommandHandler(
    IChannelRepository channelRepository, 
    IChannelMemberRepository channelMemberRepository, 
    IMemberRepository memberRepository, 
    IUnitOfWork unitOfWork) : ICommandHandler<AddMembersToChannelCommand>
{
    public async Task<Result> Handle(AddMembersToChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        bool existMember = await memberRepository.Exist(request.Id);
        if (!existMember)
        {
            throw new InvalidOperationException("Member was not found");
        }

        
        var memberChannel = ChannelMember.Create(request.ChannelId, request.Id);


        await channelMemberRepository.AddChannelMembers(memberChannel);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
