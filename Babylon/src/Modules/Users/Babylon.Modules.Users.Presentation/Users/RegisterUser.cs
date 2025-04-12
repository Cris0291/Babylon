using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Babylon.Modules.Users.Presentation.Users;
internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Users.RegisterUser, async (ISender sender, Request request) =>
        {
            await sender.Send(new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password));
        })
          .AllowAnonymous()
          .WithTags(Tags.Users);
    }
    internal sealed record Request(string Email, string FirstName, string LastName, string Password);
}
