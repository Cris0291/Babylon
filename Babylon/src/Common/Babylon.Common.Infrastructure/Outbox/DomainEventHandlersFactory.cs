using System.Collections.Concurrent;
using System.Reflection;
using Babylon.Common.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Babylon.Common.Infrastructure.Outbox;
public static class DomainEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> _handlersDictionary = new();
    public static IEnumerable<IDomainEventHandler> GetHandlers(Type type, IServiceProvider serviceProvider, Assembly assembly)
    {
        Type[] domainHandlerTypes = _handlersDictionary.GetOrAdd(
            $"{assembly.GetName().Name}-{type.Name}",
            _ =>
            {
                Type[] domainHandlerTypes = assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
                .ToArray();

                return domainHandlerTypes;
            }
            );

        List<IDomainEventHandler> handlers = [];
        foreach (Type item in domainHandlerTypes)
        {
            object domainEventHandler = serviceProvider.GetRequiredService(item);

            handlers.Add((IDomainEventHandler)domainEventHandler);
        }

        return handlers;
    }
}
