using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Babylon.Common.Infrastructure.Authorization;
internal class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _authorizationOptions;
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _authorizationOptions = options.Value;
    }
    public async override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
         AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if(policy is not null)
        {
            return policy;
        }

        AuthorizationPolicy permissionPolicy = new AuthorizationPolicyBuilder().AddRequirements(new PermissionsRequirement(policyName)).Build();

        _authorizationOptions.AddPolicy(policyName, permissionPolicy);

        return permissionPolicy;
    }
}
