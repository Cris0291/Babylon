﻿using Babylon.Common.Application.EventBus;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Services;
using Babylon.Modules.Channels.Application.Channels.BlockMemberFromChannel;
using Babylon.Modules.Channels.Application.Channels.ChannelArchiveValidation;
using Babylon.Modules.Channels.Application.Channels.DeleteMemberFormChannel;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Babylon.Modules.Channels.Application.Members.GetMemberAdmin;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.Application.Messages.AddMessageChannelReaction;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
public sealed class ChannelHub(ISender sender, IEventBus bus, IUserConnectionService connectionService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();

        if(httpContext == null)
        {
            throw new HubException("Unable to connect to requested channel or thread");
        }

        Guid channelId = Guid.TryParse((string)httpContext.Request.RouteValues["id"], out Guid id) ? id : throw new InvalidCastException("Given channel id was not correct");

        string groupName = $"{channelId}";

        string userId = Context.User?.FindFirst("sub")?.Value;

        Guid uId =  Guid.TryParse(userId, out Guid usId) ? usId : throw new InvalidOperationException("User id could not be found");

        Result<bool> isUserRegisteredInChannel = await sender.Send(new GetValidChannelQuery(uId, channelId));

        if (!isUserRegisteredInChannel.TValue)
        {
            throw new InvalidOperationException($"User does not have acces to channel");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        Result<IEnumerable<MessageResponse>> messages = await sender.Send(new GetChannelMessagesQuery(channelId));

        await Clients.Caller.SendAsync("LoadMessages", messages.TValue);

        await base.OnConnectedAsync();
    }
    public async Task SendMessage(MessageRequest req)
    {
        string groupName = $"{req.ChannelId}";

        Result<bool> isArchiveResult = await sender.Send(new ChannelArchiveValidationQuery(req.ChannelId));
        if (isArchiveResult.TValue)
        {
            throw new HubException("Channel is archive new messages cannot be added");
        }

        await bus.PublishAsync(new ChannelPublishMessageIntegrationEvent(req.ChannelId, req.MemberId, req.Message, req.PublicationDate, req.UserName, req.Avatar));

        await Clients.Group(groupName).SendAsync("ReceiveMessage", req);
    }

    public async Task RemoveUserFromGroup(RemoveUserRequest request)
    {
        string groupName = $"{request.ChannelId}";

        await ValidateAdmin(request.ChannelId, request.AdminId);

        await ValidMemeberGroup(request.TargetId, request.ChannelId);

        await RemoveOrBlockUser(request.IsBlocked, request.ChannelId, request.TargetId);

        List<string> userConnections = connectionService.GetConnections(request.TargetId);

        foreach (string connection in userConnections)
        {
            await Groups.RemoveFromGroupAsync(connection, groupName);
            await Clients.Client(connection).SendAsync("DeletedMember", groupName);
        }

        await Clients.Group(groupName).SendAsync("UserRemoved", request.TargetId);
    }
    public async Task ReactToMessage(MessageReactionRequest reaction)
    {
        Result result = await sender.Send(new AddMessageChannelReactionCommand(reaction.MemberId, reaction.MessageChannelId, reaction.Emoji));
    }

    private async Task ValidateAdmin(Guid channelId, Guid adminId)
    {
        Result<bool> isAdmin = await sender.Send(new GetMemberAdminQuery(channelId, adminId));

        if (!isAdmin.TValue)
        {
            throw new HubException("You are not authorized to remove users from this group.");
        }
    }
    private async Task ValidMemeberGroup(Guid targetId, Guid channelId)
    {
        Result<bool> isMemberOfChannel = await sender.Send(new GetValidChannelQuery(targetId, channelId));

        if (!isMemberOfChannel.TValue)
        {
            throw new HubException("Target user is not in the group.");
        }
    }
    private async Task RemoveOrBlockUser(bool isBlocked,Guid channelId, Guid targetId)
    {
        if (isBlocked)
        {
            Result blockedResult = await sender.Send(new BlockMemberFromChannelCommand(channelId, targetId));

            if (!blockedResult.IsSuccess)
            {
                throw new HubException(blockedResult.Error!.Description);
            }
        }
        else
        {
            Result deletedResult = await sender.Send(new DeleteMemberFormChannelCommand(channelId, targetId));

            if (!deletedResult.IsSuccess)
            {
                throw new HubException(deletedResult.Error!.Description);
            }
        }
    }

    public sealed record MessageRequest(Guid ChannelId, string ChannelName, Guid MemberId, string UserName, string Message, DateTime PublicationDate, string Avatar);
    public sealed record MessageReactionRequest(Guid MessageChannelId, Guid MemberId, string Emoji);
}
