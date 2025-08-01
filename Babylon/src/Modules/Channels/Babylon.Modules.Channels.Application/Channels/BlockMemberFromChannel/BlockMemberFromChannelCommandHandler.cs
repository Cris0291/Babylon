﻿using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Channels.BlockMemberFromChannel;
internal sealed class BlockMemberFromChannelCommandHandler(
    IChannelRepository channelRepository, 
    IChannelMemberRepository channelMemberRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<BlockMemberFromChannelCommand>
{
    public async Task<Result> Handle(BlockMemberFromChannelCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if (channel is null)
        {
            return Result.Failure(Error.Failure(description: "Channel was not found"));
        }
        
        if(channel.Creator != request.AdminId)
        {
            return Result.Failure(Error.Failure(description: "Only channel admin is authorized to block users from this group."));
        }

        ChannelMember? channelMember = await channelMemberRepository.GetChannelMember(request.ChannelId, request.Id);

        if (channelMember == null)
        {
            return Result.Failure(Error.Failure(description: "Member was not part of the channel"));
        }

        if (channel.IsBlocked(request.Id))
        {
            return Result.Failure(Error.Failure(description: "Member was already blocked from the channel"));
        }
        channel.BlockMember(request.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
