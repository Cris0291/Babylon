using Babylon.Common.Application.Authorization;
using Babylon.Common.Application.EventBus;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Infrastructure.Outbox;
using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Users.Application.Abstractions.Data;
using Babylon.Modules.Users.Application.Abstractions.Identity;
using Babylon.Modules.Users.Domain.Users;
using Babylon.Modules.Users.Infrastructure.Authorization;
using Babylon.Modules.Users.Infrastructure.Database;
using Babylon.Modules.Users.Infrastructure.Identity;
using Babylon.Modules.Users.Infrastructure.Inbox;
using Babylon.Modules.Users.Infrastructure.Outbox;
using Babylon.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Babylon.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connection = configuration.GetConnectionString("DataBase");

        services.AddScoped<IPermissionService, PermissionService>();

        services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));

        services.AddTransient<KeyCloakAuthDelegatingHandler>();

        services.AddHttpClient<KeyCloakClient>((serviceProvider, httpClient) =>
        {
            KeyCloakOptions options = serviceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

            httpClient.BaseAddress = new Uri(options.AdminUrl);
        }).AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        services.AddTransient<IIdentityProviderService, IdentityProviderService>();

        services.AddDbContext<UsersDbContext>((sp, options) => options.UseSqlServer(connection,
            options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users))
        .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageInterceptor>())
        .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();

        services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Users:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();

        return services;
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

           Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(t => t.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type idempotentConsumer = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, idempotentConsumer);
        }
    }
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlerTypes = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandlerType in integrationEventHandlerTypes)
        {
            services.TryAddScoped(integrationEventHandlerType);

            Type integrationEventType = integrationEventHandlerType
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type idempotentType = typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEventType);

            services.Decorate(integrationEventHandlerType, idempotentType);
        }
    }
}

