using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Armsoft.EventBus.ServiceBus
{
    public class ServiceBusEventBus : IEventBus
    {
        private readonly IServiceBusConnectionManager _serviceBusConnectionManager;
        private readonly ILogger<ServiceBusEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventBusSubscriptionsManager _subscriptionsManager;
        private readonly SubscriptionClient _subscriptionClient;

        public ServiceBusEventBus(
            IServiceBusConnectionManager serviceBusConnectionManager,
            IEventBusSubscriptionsManager subsManager,
            ILogger<ServiceBusEventBus> logger,
            IServiceProvider serviceProvider,
            IOptions<EventBusOptions> options)
        {
            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));
            _serviceBusConnectionManager = serviceBusConnectionManager;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider;
            _subscriptionsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _subscriptionClient = new SubscriptionClient(serviceBusConnectionManager.ServiceBusConnectionStringBuilder, options.Value.SubscriptionName);

            // todo sync over async - YUCK!
            RemoveDefaultRule().GetAwaiter().GetResult();
            RegisterSubscriptionClientMessageHandler();
        }

        public Task Publish(IntegrationEvent @event)
        {
            var connection = _serviceBusConnectionManager.GetConnection();
            var eventName = @event.GetType().Name;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName
            };
            return connection.SendAsync(message);
        }

        public async Task Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var containsKey = _subscriptionsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    await _subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = eventName },
                        Name = eventName
                    });
                }
                catch (ServiceBusException)
                {
                    _logger.LogInformation("Stopped adding the rule {ruleName} as it already exists.", eventName);
                }
            }
            _subscriptionsManager.AddSubscription<T, TH>();
        }

        public async Task Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;
            try
            {
                await _subscriptionClient.RemoveRuleAsync(eventName);
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogInformation("Stopped removing the rule {ruleName} as it does not exist.", eventName);
            }
            _subscriptionsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            _subscriptionsManager.Clear();
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = message.Label;
                    var messageData = Encoding.UTF8.GetString(message.Body);
                    if (await ProcessEvent(eventName, messageData))
                        await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                },
               new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            var e = exceptionReceivedEventArgs.Exception;
            _logger.LogError(e, "Message handler encountered an error while processing a message. Endpoint - {Endpoint}; EntityPath - {EntityPath}; ExecutingAction - {Action}. Message: {Message}",
                context.Endpoint,
                context.EntityPath,
                context.Action,
                e.Message);
            return Task.CompletedTask;
        }

        private async Task<bool> ProcessEvent(string eventName, string message)
        {
            if (!_subscriptionsManager.HasSubscriptionsForEvent(eventName))
                return false;
            var subscriptions = _subscriptionsManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                    if (handler == null)
                        continue;
                    var eventType = _subscriptionsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.Handle)).Invoke(handler, new[] { integrationEvent });
                }
            }
            return true;
        }

        private async Task RemoveDefaultRule()
        {
            try
            {
                await _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName);
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogInformation("Stopped removing the rule {ruleName} as it does not exist.", RuleDescription.DefaultRuleName);
            }
        }
    }
}