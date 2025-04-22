using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.CreateThread;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class CreateThread : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.CreateThread, async (ISender sender, Request request) =>
        {
            await sender.Send(new CreateThreadCommand(request.ChannelName, request.ChannelId, request.Message.UserName, request.Message.MessageText, request.Message.CreationDate, request.Message.MemberId));
        }).WithTags(Tags.Channels);
    }
    public sealed record Request(string ChannelName, Guid ChannelId, MessageThreadRequest Message);
    public sealed record MessageThreadRequest(string UserName, string MessageText, DateTime CreationDate, Guid MemberId);
}
