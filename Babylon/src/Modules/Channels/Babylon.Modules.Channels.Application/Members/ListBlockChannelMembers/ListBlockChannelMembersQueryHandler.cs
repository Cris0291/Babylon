using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Domain.Channels;

namespace Babylon.Modules.Channels.Application.Members.ListBlockChannelMembers;
internal sealed class ListBlockChannelMembersQueryHandler( IChannelRepository channelRepository) : IQueryHandler<ListBlockChannelMembersQuery, IEnumerable<BlockMemberDto>>
{
    public async Task<Result<IEnumerable<BlockMemberDto>>> Handle(ListBlockChannelMembersQuery request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);

        if (channel == null)
        {
            return Result.Failure<IEnumerable<BlockMemberDto>>(Error.Failure(description: "Requested channel was not found"));
        }

        bool isAdmin = channel.IsAdmin(request.AdminId);

        if (!isAdmin)
        {
            return Result.Failure<IEnumerable<BlockMemberDto>>(Error.Failure(description: "Administrator level permission is required to perform given action"));
        }

        
    }
}
