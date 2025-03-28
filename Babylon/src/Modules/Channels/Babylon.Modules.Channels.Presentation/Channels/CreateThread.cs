using Babylon.Common.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Channels;
internal sealed class CreateThread : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Channels.CreateThread, async (ISender sender) =>
        {
            await sender.Send();
        }).WithTags(Tags.Channels);
    }
}
