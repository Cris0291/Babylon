using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Users.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}

