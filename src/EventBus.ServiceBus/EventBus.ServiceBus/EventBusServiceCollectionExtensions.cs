using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Armsoft.EventBus.ServiceBus
{
    public static class EventBusServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBusEventBus(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IEventBus, ServiceBusEventBus>());
            return services;
        }

        public static IServiceCollection AddServiceBusEventBus(
            this IServiceCollection services,
            Action<EventBusOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            services.AddServiceBusEventBus();
            services.Configure<EventBusOptions>(setupAction);
            return services;
        }
    }
}