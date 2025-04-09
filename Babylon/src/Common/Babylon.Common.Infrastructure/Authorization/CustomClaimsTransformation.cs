using System.Security.Claims;
using Babylon.Common.Application.Authorization;
using Babylon.Common.Domain;
using Babylon.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Common.Infrastructure.Authorization;
internal sealed class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if(principal.HasClaim(c => c.Type == CustomClaims.Sub))
        {
            return principal;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        string identityId = principal.GetIdentityId();

        Result<PermissionsResponse> result =  await permissionService.GetUserPermissionsAsync(identityId);

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException("something unexpected happened");
        }

        var claimsIdentity = new ClaimsIdentity();

        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.TValue!.UserId.ToString()));

        foreach (string permission in result.TValue.Permissions)
        {
            claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
        }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}
