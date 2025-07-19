using Babylon.Common.Application.Caching;
using Babylon.Common.Application.EventBus;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Abstractions.Services;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Babylon.Modules.Channels.Application.Channels.GetChannelStateAccess;
using Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
using Babylon.Modules.Channels.Application.ThreadMessages.AddMessageThreadChannelReaction;
using Babylon.Modules.Channels.Application.ThreadMessages.AddOrRemoveMessageThreadChannelLike;
using Babylon.Modules.Channels.Application.ThreadMessages.EditThreadMessages;
using Babylon.Modules.Channels.Application.Threads.ArchiveThread;
using Babylon.Modules.Channels.Application.Threads.GetThreadChannelMessages;
using Babylon.Modules.Channels.Application.Threads.RenameThread;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
public sealed class ThreadHub(ISender sender, IEventBus bus, IUserConnectionService connectionService, ICacheService cacheService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();

        if (httpContext == null)
        {
            throw new HubException("Unable to connect to requested channel or thread");
        }
        
        Guid channelId = Guid.TryParse((string)httpContext.Request.RouteValues["channelId"], out Guid cid) 
                    ? cid 
                    : throw new InvalidOperationException("Given thread id was not correct");

        Guid threadId = Guid.TryParse((string)httpContext.Request.RouteValues["threadId"], out Guid tid) 
            ? tid 
            : throw new InvalidOperationException("Given thread id was not correct");

        string uId = Context.User?.FindFirst("sub")?.Value;

        Guid userId = Guid.TryParse(uId, out Guid usId) ? usId : throw new InvalidOperationException("User id could not be found");

        string groupName = $"{threadId}";

        Result<(bool, bool)> result = await sender.Send(new GetValidThreadChannelQuery(userId, threadId, channelId));

        if (!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        
        connectionService.AddConnection(userId, Context.ConnectionId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        Result<IEnumerable<MessageResponse>> messages = await sender.Send(new GetThreadChannelMessagesQuery(threadId));

        await Clients.Caller.SendAsync("LoadThreadMessages", new {Messages = messages.TValue, IsMute = result.TValue.Item1, IsArchiived = result.TValue.Item2});

        await base.OnConnectedAsync();
    }
    public async Task SendMessage(MessageThreadRequest req)
    {
        string groupName = $"{req.ThreadId}";
        
        await ValidateChannelAccess(req.ChannelId, req.MemberId);
        
        await Clients.Group(groupName).SendAsync("ReceiveThreadMessage", req);

        await bus.PublishAsync(new ThreadPublishMessageIntegrationEvent(req.ThreadId, req.MemberId, req.Message, req.PublicationDate, req.UserName, req.Avatar));
    }

    public async Task RenameThread(RenameThreadRequest req)
    {
        string groupName = $"{req.ThreadChannelId}";
        
        await ValidateChannelAccess(req.ChannelId, req.Id);

        Result result = await sender.Send(new RenameThreadChannelCommand(req.ThreadChannelId, req.ThreadChannelName, req.Id));
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        
        await Clients.Group(groupName).SendAsync("RenameThreadClient", new {req.ThreadChannelId, req.ThreadChannelName});
    }
    /*public async Task ArchiveThread(ArchiveRecordRequest req)
    {
        string groupName = $"{req.ThreadChannelId}";

        Result result = await sender.Send(new ArchiveThreadChannelCommand(req.ThreadChannelId, req.ChannelId, req.AdminId));
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        await Clients.Group(groupName).SendAsync("ArchiveThreadClient", new {req.ThreadChannelId});
    }*/
    public async Task ReactToThreadMessage(MessageThreadReactionRequest reaction)
    {
        string groupName = $"{reaction.ThreadChannelId}";
        
        await ValidateChannelAccess(reaction.ChannelId,reaction.MemberId);

        Result result = await sender.Send(new AddMessageThreadChannelReactionCommand(reaction.MemberId, reaction.MessageThreadChannelId, reaction.ThreadChannelId, reaction.Emoji));

        if (!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description) ;
        }

        await Clients.Group(groupName).SendAsync("AddThreadReaction", new { reaction.Emoji, reaction.MessageThreadChannelId, reaction.MemberId });
    }

    public async Task AddOrRemoveThreadMessageLike(MessageThreadLikeRequest request)
    {
        string groupName = $"{request.ThreadChannelId}";
        
        await ValidateChannelAccess(request.ChannelId, request.Id);
    
        Result<int> result = await sender.Send(new AddOrRemoveMessageThreadChannelLikeCommand(request.Id, request.ThreadMessageId, request.ThreadChannelId, request.Like));
    
        if (!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description) ;
        }
    
        await Clients.Group(groupName).SendAsync("AddOrRemoveThreadMessageLikeClient", new { request.Like, request.ThreadMessageId, result.TValue });
    }
    public async Task NotifyTyping(TypingNotification request)
    {
        string groupName = $"{request.ThreadChannelId}";
        
        await ValidateChannelAccess(request.ChannelId, request.Id);

        await Clients.Group(groupName).SendAsync("NotifyUserThreadMessage", new {Notification = $"{request.UserName} is typing"});
    }
    public async Task EditThreadMessage(EditThreadMessageRequest request)
    {
        string groupName = $"{request.ThreadChannelId}";
        
        Result result = await sender.Send(new EditThreadMessageQuery(request.MessageThreadChannelId, request.Message));
        
        if (!result.IsSuccess)
        {
            await Clients.Caller.SendAsync("EditThreadMessage", new { request.Message, request.MessageThreadChannelId, request.ThreadChannelId, Error = "Message could not be edited" });
            return;
        }
        
        await Clients.Group(groupName).SendAsync("EditThreadMessage", new { request.Message, request.MessageThreadChannelId, request.ThreadChannelId });
    }
    
    private async Task ValidateChannelAccess(Guid channelId, Guid id)
    {
        ChannelAccessStateDto? cacheResult = await cacheService.GetAsync<ChannelAccessStateDto>($"channelState-{channelId}-{id}");

        if (cacheResult == null)
        {
            cacheResult = (await sender.Send(new GetChannelStateAccessQuery(channelId, id))).TValue;
            await cacheService.SetAsync($"channelState-{channelId}-{id}", cacheResult);
        }

        if (cacheResult!.Type == Domain.Channels.ChannelType.Archived)
        {
            throw new HubException("Channel is archive new messages cannot be added");
        }

        if (cacheResult .IsMute)
        {
            throw new HubException("You are currently mute on this channel");
        }
        
        if (cacheResult .IsBlocked)
        {
            throw new HubException("You are currently blocked on this channel");
        }
    }
    public sealed record MessageThreadRequest(Guid ThreadId, string ThreadName, Guid MemberId, string UserName, string Message, DateTime PublicationDate, string Avatar, Guid ChannelId);
    public sealed record RenameThreadRequest(Guid ThreadChannelId, string ThreadChannelName, Guid Id, Guid ChannelId);
    public sealed record ArchiveRecordRequest(Guid ThreadChannelId, Guid ChannelId, Guid AdminId);
    public sealed record MessageThreadReactionRequest(Guid MessageThreadChannelId, Guid MemberId, string Emoji, Guid ThreadChannelId, Guid ChannelId);
    public sealed record MessageThreadLikeRequest(Guid Id, Guid ThreadMessageId, bool Like, Guid ThreadChannelId, Guid ChannelId);
    public sealed record TypingNotification(Guid ThreadChannelId, string UserName, Guid ChannelId, Guid Id);
    public sealed record EditThreadMessageRequest(Guid MessageThreadChannelId, string Message, Guid ThreadChannelId);
}
