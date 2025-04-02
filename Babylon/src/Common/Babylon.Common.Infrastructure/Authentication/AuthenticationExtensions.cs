using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Common.Infrastructure.Authentication;
public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationInternals(this IServiceCollection services)
    {
        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();

        services.AddHttpContextAccessor();

        services.ConfigureOptions<JwtBearerConfigureOptions>();

        return services;
    }
}
