using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.ArchiveChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class ArchiveChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.Channels.ArchiveChannel, async (ISender sender, Guid id) =>
        {
            await sender.Send(new ArchiveChannelCommand(id));
        }).WithTags(Tags.Channels);
    }
}
