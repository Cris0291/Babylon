using Babylon.Common.Application.EventBus;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
using Babylon.Modules.Channels.Application.ThreadMessages.AddMessageThreadChannelReaction;
using Babylon.Modules.Channels.Application.ThreadMessages.AddOrRemoveMessageThreadChannelLike;
using Babylon.Modules.Channels.Application.Threads.ArchiveThread;
using Babylon.Modules.Channels.Application.Threads.GetThreadChannelMessages;
using Babylon.Modules.Channels.Application.Threads.RenameThread;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
public sealed class ThreadHub(ISender sender, IEventBus bus) : Hub
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();

        if (httpContext == null)
        {
            throw new HubException("Unable to connect to requested channel or thread");
        }

        Guid threadId = Guid.TryParse((string)httpContext.Request.RouteValues["threadId"], out Guid tid) 
            ? tid 
            : throw new InvalidOperationException("Given thread id was not correct");

        string uId = Context.User?.FindFirst("sub")?.Value;

        Guid userId = Guid.TryParse(uId, out Guid usId) ? usId : throw new InvalidOperationException("User id could not be found");

        string groupName = $"{threadId}";

        Result<bool> result = await sender.Send(new GetValidThreadChannelQuery(userId, threadId));

        if (!result.TValue)
        {
            throw new InvalidOperationException($"User does not have acces to channel");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        Result<IEnumerable<MessageResponse>> messages = await sender.Send(new GetThreadChannelMessagesQuery(threadId));

        await Clients.Caller.SendAsync("LoadThreadMessages", messages.TValue);

        await base.OnConnectedAsync();
    }
    public async Task SendMessage(MessageThreadRequest req)
    {
        string groupName = $"{req.ThreadId}";

        await bus.PublishAsync(new ThreadPublishMessageIntegrationEvent(req.ThreadId, req.MemberId, req.Message, req.PublicationDate, req.UserName, req.Avatar));

        await Clients.Group(groupName).SendAsync("ReceiveThreadMessage", req);
    }

    public async Task RenameThread(RenameThreadRequest req)
    {
        string groupName = $"{req.ThreadChannelId}";

        Result result = await sender.Send(new RenameThreadChannelCommand(req.ThreadChannelId, req.ThreadChannelName, req.Id));
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        
        await Clients.Group(groupName).SendAsync("RenameThreadClient", new {req.ThreadChannelId, req.ThreadChannelName});
    }
    public async Task ArchiveThread(ArchiveRecordRequest req)
    {
        string groupName = $"{req.ThreadChannelId}";

        Result result = await sender.Send(new ArchiveThreadChannelCommand(req.ThreadChannelId, req.ChannelId, req.AdminId));
        if(!result.IsSuccess)
        {
            throw new HubException(result.Error?.Description);
        }
        await Clients.Group(groupName).SendAsync("ArchiveThreadClient", new {req.ThreadChannelId});
    }
    public async Task ReactToThreadMessage(MessageThreadReactionRequest reaction)
    {
        string groupName = $"{reaction.ThreadChannelId}";

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

        await Clients.Group(groupName).SendAsync("NotifyUserThreadMessage", new {Notification = $"{request.UserName} is typing"});
    }
    public sealed record MessageThreadRequest(Guid ThreadId, string ThreadName, Guid MemberId, string UserName, string Message, DateTime PublicationDate, string Avatar);
    public sealed record RenameThreadRequest(Guid ThreadChannelId, string ThreadChannelName, Guid Id);
    public sealed record ArchiveRecordRequest(Guid ThreadChannelId, Guid ChannelId, Guid AdminId);
    public sealed record MessageThreadReactionRequest(Guid MessageThreadChannelId, Guid MemberId, string Emoji, Guid ThreadChannelId);
    public sealed record MessageThreadLikeRequest(Guid Id, Guid ThreadMessageId, bool Like, Guid ThreadChannelId);
    public sealed record TypingNotification(Guid ThreadChannelId, string UserName);
}
