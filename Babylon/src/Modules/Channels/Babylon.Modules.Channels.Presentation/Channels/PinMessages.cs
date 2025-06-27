using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Messages.PinMessage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;

internal sealed class PinMessages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.PinChannelMessage, async (ISender sender, Guid channelId, Guid messageChannelId, Request request) =>
        {
            await sender.Send(new PinMessageCommand(channelId, messageChannelId, request.MemberId, request.Pin));
        }).WithTags(Tags.Channels);
    }
    internal sealed record Request(Guid MemberId, bool Pin);
}
