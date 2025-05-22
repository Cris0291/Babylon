using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.RenameChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class RenameChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.RenameChannel, async (ISender sender, Guid id, Request request) =>
        {
            await sender.Send(new RenameChannelCommand(id, request.Name));
        }).WithTags(Tags.Channels);
    }
    internal sealed record Request(string Name);
}
