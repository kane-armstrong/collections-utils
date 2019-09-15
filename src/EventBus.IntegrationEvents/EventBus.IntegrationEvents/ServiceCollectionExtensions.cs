using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Armsoft.EventBus.IntegrationEvents
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIntegrationEventLogService(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Scoped<IIntegrationEventLogService, IntegrationEventLogService>());
            return services;
        }

        public static IServiceCollection AddIntegrationEventLogService(
            this IServiceCollection services,
            Action<IntegrationEventLogContextOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            services.AddIntegrationEventLogService();
            services.Configure<IntegrationEventLogContextOptions>(setupAction);
            return services;
        }

        public static IServiceCollection AddIntegrationEventLogContext(this IServiceCollection services, Action<IntegrationEventLogContextOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            services.Configure<IntegrationEventLogContextOptions>(setupAction);
            var options = new IntegrationEventLogContextOptions();
            setupAction.Invoke(options);
            services.AddSingleton(options);
            services.AddDbContext<IntegrationEventLogContext>(builder =>
            {
                options.ConfigureDbContext?.Invoke(builder);
            });
            return services;
        }
    }
}