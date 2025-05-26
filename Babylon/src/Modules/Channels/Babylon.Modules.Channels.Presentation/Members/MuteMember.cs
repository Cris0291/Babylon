using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Members.MuteMember;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Channels.Presentation.Members;
internal sealed class MuteMember : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.Channels.MuteMember, async (ISender sender, Guid id, Guid channelId, Request request) =>
        {
            await sender.Send(new MuteMemberCommand(request.AdminId, id, channelId));
        }).WithTags(Tags.Channels);
    }
    internal sealed record Request(Guid AdminId);
}
