using Babylon.Common.Application.EventBus;
using Babylon.Common.Application.Messaging;
using Babylon.Common.Infrastructure.Outbox;
using Babylon.Common.Presentation.Endpoints;
using Babylon.Modules.Channels.Application.Abstractions.Data;
using Babylon.Modules.Channels.Application.Abstractions.Services;
using Babylon.Modules.Channels.Domain.Channels;
using Babylon.Modules.Channels.Domain.Members;
using Babylon.Modules.Channels.Domain.MessageChannels;
using Babylon.Modules.Channels.Infrastructure.Abstractions.Services;
using Babylon.Modules.Channels.Infrastructure.Channels;
using Babylon.Modules.Channels.Infrastructure.Database;
using Babylon.Modules.Channels.Infrastructure.Inbox;
using Babylon.Modules.Channels.Infrastructure.Members;
using Babylon.Modules.Channels.Infrastructure.MessageChannels;
using Babylon.Modules.Channels.Infrastructure.Outbox;
using Babylon.Modules.Channels.IntegrationEvents;
using Babylon.Modules.Users.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Babylon.Modules.Channels.Infrastructure;
public static class ChannelsModule
{
    public static IServiceCollection AddChannelsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDomainEventHandlers();
        services.AddIntegrationEventHandlers();
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<ChannelPublishMessageIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<UserRegisteredIntegrationEvent>>();
    }
    private static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ChannelsDbContext>((sp, options) => options.UseSqlServer(databaseConnectionString,
            options => options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Channels))
        .AddInterceptors(sp.GetRequiredService<InsertOutboxMessageInterceptor>())
        .UseSnakeCaseNamingConvention());
            

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ChannelsDbContext>());
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IChannelMemberRepository, ChannelMemberRepository>();
        services.AddScoped<IMessageChannelRepository, MessageChannelRepository>();
        services.AddSingleton<IUserConnectionService, UserConnectionService>();

        services.Configure<OutboxOptions>(configuration.GetSection("Channels:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Channels:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();

        return services;
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlerTypes = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlerTypes)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type idempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, idempotentHandler);
        }
    }
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlerTypes = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlerTypes)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type idempotentHandler = typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, idempotentHandler);
        }
    }
}
