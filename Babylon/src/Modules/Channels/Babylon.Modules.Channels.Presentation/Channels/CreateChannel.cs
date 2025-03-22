using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.CreateChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;

internal class CreateChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.CreateChannel, async (ISender sender, Request request) =>
        {
            await sender.Send(new CreateChannelCommand(request.ChannelName, request.IsPublicChannel, request.MemberId));
        }).WithTags(Tags.Channels);
    }
    internal sealed record Request(string ChannelName, bool IsPublicChannel, Guid MemberId);
}
    


