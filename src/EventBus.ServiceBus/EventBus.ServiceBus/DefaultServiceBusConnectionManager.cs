using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using System;

namespace Armsoft.EventBus.ServiceBus
{
    public class DefaultServiceBusConnectionManager : IServiceBusConnectionManager
    {
        private ITopicClient _topicClient;

        public DefaultServiceBusConnectionManager(IOptions<EventBusConnectionOptions> options)
        {
            if (options?.Value == null)
                throw new ArgumentNullException(nameof(options));
            ServiceBusConnectionStringBuilder = new ServiceBusConnectionStringBuilder(options.Value.TopicConnectionString);
            _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        public ITopicClient GetConnection()
        {
            if (_topicClient.IsClosedOrClosing)
                _topicClient = new TopicClient(ServiceBusConnectionStringBuilder, RetryPolicy.Default);
            return _topicClient;
        }
    }
}