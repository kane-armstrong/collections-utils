using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Armsoft.EventBus
{
    public static class SubscriptionManagerServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemorySubscriptionsManager(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>());
            return services;
        }
    }
}