using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;
public class AddMemberHub  : Hub
{
    public async Task NotifyUser(string userId, NotificationAddedUser notificationAddedUser)
    {
        await Clients.User(userId).SendAsync("NotifyAddedUser", notificationAddedUser);
    }
    public sealed record NotificationAddedUser(Guid ChannelId);
}
