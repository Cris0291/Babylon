﻿using System.Collections.Concurrent;
using System.Reflection;
using Babylon.Common.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Common.Infrastructure.Inbox;
public static class IntegrationEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> _handlersDictionary = new();
    public static IEnumerable<IIntegrationEventHandler> GetHandlers(Type type, IServiceProvider serviceProvider, Assembly assembly)
    {
        Type[] integrationEventHandlerTypes = _handlersDictionary.GetOrAdd(
            $"{assembly.GetName().Name}-{type.Name}",
            _=>
            {
                Type[] integrationEventHandlers = assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler<>).MakeGenericType(type)))
                .ToArray();

                return integrationEventHandlers;
            });

        List<IIntegrationEventHandler> handlers = [];
        foreach (Type integrationEventHandlerType in integrationEventHandlerTypes)
        {
            object integrationEventHandler = serviceProvider.GetRequiredService(integrationEventHandlerType);

            handlers.Add((IIntegrationEventHandler)integrationEventHandler);
        }

        return handlers;
    }
}
