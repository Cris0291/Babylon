using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.DeleteMemberFormChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class DeleteMemeberFromChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Channels.DeleteMemberFromChannel, async (ISender sender, Guid channelId, Guid memberId) =>
        {
            await sender.Send(new DeleteMemberFormChannelCommand(channelId, memberId));
        }).WithTags(Tags.Channels);
    }
}
