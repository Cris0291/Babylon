using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.ThreadMessages.PinThreadMessage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Threads;

internal sealed class PinThreadMessages : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.PinThreadChannelMessage, async (ISender sender, Guid threadChannelId, Guid messageThreadChannelId, Request request) =>
        {
            await sender.Send(new PinThreadMessageCommand(threadChannelId, messageThreadChannelId, request.MemberId, request.Pin));
        });
    }
    internal sealed record Request(Guid MemberId, bool Pin);
}
