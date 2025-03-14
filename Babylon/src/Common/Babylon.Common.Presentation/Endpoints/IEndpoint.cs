using Microsoft.AspNetCore.Routing;

namespace Babylon.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

