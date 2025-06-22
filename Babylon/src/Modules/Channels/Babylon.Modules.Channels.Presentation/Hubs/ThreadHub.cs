using Babylon.Common.Application.EventBus;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Babylon.Modules.Channels.Application.Members.GetValidThreadChannel;
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

        await sender.Send(new RenameThreadChannelCommand(req.ThreadChannelId, req.ThreadChannelName, req.Id));
        
        await Clients.Group(groupName).SendAsync("RenameThreadClient", new {req.ThreadChannelId, req.ThreadChannelName});
    }
    public sealed record MessageThreadRequest(Guid ThreadId, string ThreadName, Guid MemberId, string UserName, string Message, DateTime PublicationDate, string Avatar);
    public sealed record RenameThreadRequest(Guid ThreadChannelId, string ThreadChannelName, Guid Id);
}
