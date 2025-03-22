using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Channels.GetChannels;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal class GetChannels : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Channels.GetChannels, async (ISender sender,Request request) =>
        {
            await sender.Send(new GetChannelsQuery(request.Name, request.Type));
        }).WithTags(Tags.Channels);
            
    }
    internal sealed record Request(string Name, string Type);
}


