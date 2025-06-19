using System.Data.Common;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Application.Threads.AddMembersToThread;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.MessageThreadChannels;
using Babylon.Modules.Channels.Domain.ThreadChannels;
using Dapper;
using MediatR;

namespace Babylon.Modules.Channels.Application.Channels.CreateThread;
internal sealed class CreateThreadCommandHandler(
    IUnitOfWork unitOfWork, 
    IChannelRepository channelRepository, 
    IThreadChannelRepository threadChannelRepository, 
    IMessageThreadChannelRepository messageRepository,
    IDbConnectionFactory dbConnectionFactory,
    ISender sender) : ICommandHandler<CreateThreadCommand>
{
    public async Task<Result> Handle(CreateThreadCommand request, CancellationToken cancellationToken)
    {
        Channel? channel = await channelRepository.GetChannel(request.ChannelId);
        if (channel is null)
        {
            throw new InvalidOperationException("Channel was not found");
        }

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
               cm.id AS {nameof(MemberDto.Id)}
            FROM channels.channel_members cm
            WHERE cm.channel_id = @ChannelId
            """;

        IEnumerable<MemberDto> members = await connection.QueryAsync<MemberDto>(sql, new { request.ChannelId});

        var threadChannel = ThreadChannel.Create(request.ChannelName, request.ChannelId);
        await threadChannelRepository.Insert(threadChannel);

        var message = MessageThreadChannel.Create(threadChannel.ThreadChannelId, request.MemberId, request.MessageText, request.UserName, request.Avatar, request.CreationDate);
        await messageRepository.Insert(message);

        await sender.Send(new AddMemberToThreadCommand(members, threadChannel.ThreadChannelId), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
