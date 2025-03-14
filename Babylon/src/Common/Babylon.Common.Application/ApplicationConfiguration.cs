using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] assemblyModules)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblyModules));
        services.AddValidatorsFromAssemblies(assemblyModules, includeInternalTypes: true);

        return services;
    }
}
    

