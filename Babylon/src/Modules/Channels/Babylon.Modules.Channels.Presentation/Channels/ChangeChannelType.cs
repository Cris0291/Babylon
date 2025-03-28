using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.ChangeChannelType;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class ChangeChannelType : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Channels.ChangeChannelType, async (ISender sender, Guid id, Request request) =>
        {
            await sender.Send(new ChangeChannelTypeCommand(id, request.ChannelCreator, request.ChannelType));
        }).WithTags(Tags.Channels);
    }
    internal sealed record Request(Guid ChannelCreator, string ChannelType);
}
