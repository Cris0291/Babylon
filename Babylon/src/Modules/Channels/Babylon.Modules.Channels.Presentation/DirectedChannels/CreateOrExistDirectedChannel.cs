using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.DirectedChannels.CreateOrExistDirectedChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.DirectedChannels;

internal sealed class CreateOrExistDirectedChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.DirectChannels.CreateDirectChannel, async (ISender sender, Request request) =>
        {
            await sender.Send(new CreateOrExistDirectedChannelCommand(request.Creator, request.Participant));
        }).WithTags(Tags.DirectChannels);
    }
    internal sealed record Request(Guid Creator, Guid Participant);
}
