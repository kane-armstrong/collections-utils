using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Armsoft.EventBus.ServiceBus
{
    public static class ConnectionManagerServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultServiceBusConnectionManager(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IServiceBusConnectionManager, DefaultServiceBusConnectionManager>());
            return services;
        }

        public static IServiceCollection AddDefaultServiceBusConnectionManager(
            this IServiceCollection services,
            Action<EventBusConnectionOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            services.AddDefaultServiceBusConnectionManager();
            services.Configure<EventBusConnectionOptions>(setupAction);
            return services;
        }
    }
}