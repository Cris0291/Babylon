using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.AddMembersToChannel;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class AddMembersToChannel : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.AddMembersToChannel, async (ISender sender, Guid id, Request request) =>
        {
            await sender.Send(new AddMembersToChannelCommand(id, request.Members));
        }).WithTags(Tags.Channels);
    }
    internal sealed record Request(IEnumerable<Guid> Members);
}
