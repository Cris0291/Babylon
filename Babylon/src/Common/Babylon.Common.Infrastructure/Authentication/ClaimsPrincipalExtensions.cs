using System.Security.Claims;

namespace Babylon.Common.Infrastructure.Authentication;
public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        string userId = claimsPrincipal?.FindFirst(CustomClaims.Sub)?.Value;

        return Guid.TryParse(userId, out Guid id) ? id : throw new InvalidOperationException("User id could not be found");
    }
    public static string GetIdentityId(this ClaimsPrincipal? claimsPrincipal)
    {
        return claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User identity could not be found");
    }
    public static HashSet<string> GetPermissions(this ClaimsPrincipal? claimsPrincipal)
    {
        IEnumerable<Claim> permissions = claimsPrincipal?.FindAll(CustomClaims.Permission) ?? throw new InvalidOperationException("Permissions are unavailable");

        return permissions.Select(C => C.Value).ToHashSet();
    }
}
