﻿using System.Data.Common;
using Babylon.Common.Application.Caching;
using Babylon.Common.Application.Data;
using Babylon.Common.Application.EventBus;
using Babylon.Common.Infrastructure.Authentication;
using Babylon.Common.Infrastructure.Authorization;
using Babylon.Common.Infrastructure.Caching;
using Babylon.Common.Infrastructure.Data;
using Babylon.Common.Infrastructure.Outbox;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using StackExchange.Redis;

namespace Babylon.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        string connectionString, 
        Action<IRegistrationConfigurator>[] moduleConfigureConsumers,
        string redisConnectionString)
    {
        SqlClientFactory sqlDataSource = SqlClientFactory.Instance;
        DbDataSource sqlDbSource = sqlDataSource.CreateDataSource(connectionString);
        services.TryAddSingleton(sqlDbSource);

        services.AddAuthenticationInternals();

        services.AddAuthorizationInternals();

        services.TryAddScoped<IEventBus, EventBus.EventBus>();

        services.TryAddSingleton<InsertOutboxMessageInterceptor>();

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options => 
                 options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer));
        }
        catch
        {
            services.AddDistributedMemoryCache();
        }

        services.TryAddSingleton<ICacheService, CacheService>();

        services.AddMassTransit(configure =>
        {
            foreach (Action<IRegistrationConfigurator> configureConsumer in moduleConfigureConsumers)
            {
                configureConsumer(configure);
            }

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        return services;
    }
}
    

