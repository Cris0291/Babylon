using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.DeleteChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class DeleteChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Channels.DeleteChannel, async (ISender sender, Guid id) =>
        {
            await sender.Send(new DeleteChannelCommand(id));
        }).WithTags(Tags.Channels);
    }
}
