using Babylon.Common.Domain;
using Babylon.Modules.Channels.Application.Members.GetBlockedMember;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Babylon.Modules.Channels.Presentation.Hubs;

public sealed  class DirectedChannelHub(ISender sender) : Hub
{
    public override async Task OnConnectedAsync()
    {
        HttpContext? httpContext = Context.GetHttpContext();

        if (httpContext == null)
        {
            throw new HubException("Unable to connect to requested channel or thread");
        }

       Guid directedChannelId =  Guid.TryParse((string)httpContext.Request.RouteValues["id"], out Guid id) ? id : throw new HubException("Given channel id was not correct");
       
       string groupName = $"{directedChannelId}";

       string userId = Context.User?.FindFirst("sub")?.Value;

       Guid uId = Guid.TryParse(userId, out Guid usId) ? usId : throw new HubException("User id could not be found");

       Guid participantId = Guid.TryParse(httpContext.Request.Query["participant"], out Guid pId) ? pId : throw new HubException("Participant id could not be found");

       Result<bool> result = await sender.Send(new GetBlockedMemberQuery(uId, participantId));
    }
}
