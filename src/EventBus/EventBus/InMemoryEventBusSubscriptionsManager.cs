using System;
using System.Collections.Generic;
using System.Linq;

namespace Armsoft.EventBus
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public bool IsEmpty => !_handlers.Keys.Any();

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public void Clear() => _handlers.Clear();

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            AddSubscriptionHandler(typeof(TH), eventName);
            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));
        }

        public void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var eventName = GetEventKey<T>();
            RemoveSubscriptionHandler(eventName, handlerToRemove);
        }

        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent => HasSubscriptionsForEvent(GetEventKey<T>());

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent => GetHandlersForEvent(GetEventKey<T>());

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public string GetEventKey<T>() => typeof(T).Name;

        private void AddSubscriptionHandler(Type handlerType, string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
                throw new ArgumentException($"Handler type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
        }

        private void RemoveSubscriptionHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove == null)
                return;
            _handlers[eventName].Remove(subsToRemove);
            if (_handlers[eventName].Any())
                return;
            _handlers.Remove(eventName);
            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (eventType != null)
                _eventTypes.Remove(eventType);
            RaiseOnEventRemoved(eventName);
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            if (handler != null)
                OnEventRemoved?.Invoke(this, eventName);
        }

        private SubscriptionInfo FindSubscriptionToRemove<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T> =>
            FindSubscriptionToRemove(GetEventKey<T>(), typeof(TH));

        private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type handlerType) =>
            !HasSubscriptionsForEvent(eventName) ? null : _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }
}