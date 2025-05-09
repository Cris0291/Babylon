using Babylon.Common.Application.EventBus;
using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Channels.GetChannelMessages;
using Babylon.Modules.Channels.Application.Members.GetValidChannel;
using Babylon.Modules.Channels.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
public sealed class ChannelHub(ISender sender, IEventBus bus) : Hub
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();

        if(httpContext == null)
        {
            throw new HubException("Unable to connect to requested channel or thread");
        }

        Guid channelId = Guid.TryParse((string)httpContext.Request.RouteValues["id"], out Guid id) ? id : throw new InvalidCastException("Given channel id was not correct");

        string channelName = httpContext.Request.Query["name"].Single();

        string groupName = $"{channelName}-{channelId}";

        string userId = Context.User?.FindFirst("sub")?.Value;

        Guid uId =  Guid.TryParse(userId, out Guid usId) ? usId : throw new InvalidOperationException("User id could not be found");

        Result<bool> isUserRegisteredInChannel = await sender.Send(new GetValidChannelQuery(uId, channelId));

        if (!isUserRegisteredInChannel.TValue)
        {
            throw new InvalidOperationException($"User does not have acces to channel: {channelName}");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        Result<IEnumerable<MessageResponse>> messages = await sender.Send(new GetChannelMessagesQuery(channelId));

        await Clients.Caller.SendAsync("LoadMessages", messages.TValue);

        await base.OnConnectedAsync();
    }
    public async Task SendMessage(MessageRequest req)
    {
        string groupName = $"{req.ChannelName}-{req.ChannelId}";

        await bus.PublishAsync(new ChannelPublishMessageIntegrationEvent(req.ChannelId, req.MemberId, req.Message, req.PublicationDate, req.UserName, req.Avatar));

        await Clients.Group(groupName).SendAsync("ReceiveMessage", req);
    }

    public async Task RemoveUserFromGroup(RemoveUserRequest request)
    {

    }

    public sealed record MessageRequest(Guid ChannelId, string ChannelName, Guid MemberId, string UserName, string Message, DateTime PublicationDate, string Avatar);
}
