using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
internal sealed class ChannelHub() : Hub<IChannelClient>
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();

        if(httpContext == null)
        {
            throw new HubException("Unable to connect to requested channel or thread");
        }

        string channelId = (string)httpContext.Request.RouteValues["id"];
        string channelName = httpContext.Request.Query["name"].Single();
        string groupName = $"{channelName}-{channelId}";

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await base.OnConnectedAsync();
    }
    public async Task SendMessage(string message)
    {

    }
}
