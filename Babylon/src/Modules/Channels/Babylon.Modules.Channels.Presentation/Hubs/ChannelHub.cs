using Babylon.Common.Application.Caching;
using Babylon.Common.Application.EventBus;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Services;
using Babylon.Modules.Channels.Application.Channels.ArchiveChannel;
using Babylon.Modules.Channels.Application.Channels.BlockMemberFromChannel;
using Babylon.Modules.Channels.Application.Channels.ChannelArchiveValidation;
using Babylon.Modules.Channels.Application.Channels.DeleteMemberFormChannel;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
using Babylon.Modules.Channels.Application.Channels.RenameChannel;
using Babylon.Modules.Channels.Application.Members.GetMemberAdmin;
using Babylon.Modules.Channels.Application.Members.GetMemberMute;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.Application.Messages.AddMessageChannelReaction;
using Babylon.Modules.Channels.Application.Messages.AddOrRemoveMessageChannelLike;
using Babylon.Modules.Channels.Application.Messages.EditMessageChannel;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
public sealed class ChannelHub(ISender sender, IEventBus bus, IUserConnectionService connectionService, ICacheService cacheService) : Hub
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

        Result<(bool, bool)> isUserRegisteredInChannel = await sender.Send(new GetValidChannelQuery(uId, channelId));

        if (!isUserRegisteredInChannel.IsSuccess)
        {
            throw new HubException(isUserRegisteredInChannel.Error?.Description);
        }
        
        connectionService.AddConnection(uId, Context.ConnectionId);

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        Result<IEnumerable<MessageResponse>> messages = await sender.Send(new GetChannelMessagesQuery(uId, channelId));

        await Clients.Caller.SendAsync("LoadMessages", new {Messages = messages.TValue, IsMute = isUserRegisteredInChannel.TValue.Item1, IsArchiived = isUserRegisteredInChannel.TValue.Item2});

        await base.OnConnectedAsync();
    }
    public async Task SendMessage(MessageRequest req)
    {
        string groupName = $"{req.ChannelId}";

        ChannelAccessStateDto channelState;

        ChannelAccessStateDto? cacheResult = await cacheService.GetAsync<ChannelAccessStateDto>($"channelState-{req.ChannelId}-{req.Id}");

        channelState = cacheResult != null ? cacheResult : (await sender.Send(new GetChannelStateAccessQuery(req.ChannelId, req.Id))).TValue;

        if (channelState!.Type == Domain.Channels.ChannelType.Archived)
        {
            throw new HubException("Channel is archive new messages cannot be added");
        }

        if (channelState.IsMute)
        {
            throw new HubException("You are currently mute on this channel");
        }
        
        if (channelState.IsBlocked)
        {
            throw new HubException("You are currently blocked on this channel");
        }

        await Clients.Group(groupName).SendAsync("ReceiveMessage", req);

        await bus.PublishAsync(new ChannelPublishMessageIntegrationEvent(req.ChannelId, req.Id, req.Message, req.PublicationDate, req.UserName, req.Avatar));
    }

    public async Task RemoveUserFromGroup(RemoveUserRequest request)
    {
        string groupName = $"{request.ChannelId}";

        Result result = await sender.Send(new DeleteMemberFormChannelCommand(request.ChannelId, request.TargetId, request.AdminId));
        
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error!.Description);
        }

        List<string> userConnections = connectionService.GetConnections(request.TargetId);

        foreach (string connection in userConnections)
        {
            await Groups.RemoveFromGroupAsync(connection, groupName);
            await Clients.Client(connection).SendAsync("DeletedMember", groupName);
        }

        await Clients.Group(groupName).SendAsync("UserRemoved", request.TargetId);
    }

    public async Task BlockUserFromGroup(BlockChannelMemberRequest request)
    {
        string groupName = $"{request.ChannelId}";
        
        Result result = await sender.Send(new BlockMemberFromChannelCommand(request.ChannelId, request.TargetId, request.AdminId));
        
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error!.Description);
        }
        
        List<string> userConnections = connectionService.GetConnections(request.TargetId);

        foreach (string connection in userConnections)
        {
            await Clients.Client(connection).SendAsync("BlockedMember", groupName);
        }
    }
    public async Task ReactToMessage(MessageReactionRequest reaction)
    {
        string groupName = $"{reaction.ChannelId}";

        Result result = await sender.Send(new AddMessageChannelReactionCommand(reaction.MemberId, reaction.MessageChannelId, reaction.ChannelId, reaction.Emoji));

        if (!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description) ;
        }

        await Clients.Group(groupName).SendAsync("AddReaction", new { reaction.Emoji, reaction.MessageChannelId, reaction.MemberId });
    }
    public async Task AddOrRemoveLike(MessageLikeRequest reaction)
        {
            string groupName = $"{reaction.ChannelId}";
    
            Result<int> result = await sender.Send(new AddOrRemoveMessageChannelLikeCommand(reaction.Id, reaction.MessageId, reaction.ChannelId, reaction.like));
    
            if (!result.IsSuccess)
            {
                throw new HubException(result.Error?.Description) ;
            }
    
            await Clients.Group(groupName).SendAsync("AddOrRemoveLike", new { reaction.like, reaction.MessageId, result.TValue });
        }
    public async Task EditMessage(EditMessageRequest request)
    {
        string groupName = $"{request.ChannelId}";

        Result result = await sender.Send(new EditMessageChannelCommand(request.MessageChannelId, request.ChannelId, request.Id, request.Message));

        if (!result.IsSuccess)
        {
            await Clients.Caller.SendAsync("EditMessage", new { request.Message, request.MessageChannelId, request.ChannelId, Error = "Message could not be edited" });
            return;
        }

        await Clients.Group(groupName).SendAsync("EditMessage", new { request.Message, request.MessageChannelId, request.ChannelId });
    }
    public async Task NotifyTyping(TypingNotification request)
    {
        string groupName = $"{request.ChannelId}";

        await Clients.Group(groupName).SendAsync("NotifyUserMessage", new {Notification = $"{request.UserName} is typing"});
    }
    public async Task RenameChannel(RenameChannelRequest request)
    {
        string groupName = $"{request.ChannelId}";
        
        Result result = await sender.Send(new RenameChannelCommand(request.ChannelId, request.Name, request.Id));
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        
        await Clients.Group(groupName).SendAsync("RenameChannelClient", new {request.ChannelId, request.Name});
    }

    public async Task ArchiveChannel(ArchiveChannelRequest request)
    {
        string groupName = $"{request.ChannelId}";

        Result result = await sender.Send(new ArchiveChannelCommand(request.ChannelId, request.AdminId));
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        await Clients.Group(groupName).SendAsync("ArchiveChannelClient", new {request.ChannelId});
    }

    private async Task ValidateAdmin(Guid channelId, Guid adminId)
    {
        Result<bool> isAdmin = await sender.Send(new GetMemberAdminQuery(channelId, adminId));

        if (!isAdmin.TValue)
        {
            throw new HubException("You are not authorized to remove users from this group.");
        }
    }
    

    public sealed record MessageRequest(Guid ChannelId, string ChannelName, Guid Id, string UserName, string Message, DateTime PublicationDate, string Avatar);
    public sealed record MessageReactionRequest(Guid MessageChannelId, Guid MemberId, string Emoji, Guid ChannelId);
    public sealed record MessageLikeRequest(Guid Id, Guid MessageId, bool like, Guid ChannelId);
    public sealed record TypingNotification(Guid ChannelId, string UserName);
    public sealed record RenameChannelRequest(Guid ChannelId, string Name, Guid Id);
    public sealed record ArchiveChannelRequest(Guid ChannelId, Guid AdminId);
    public sealed record BlockChannelMemberRequest(Guid ChannelId, Guid AdminId, Guid TargetId);
}
