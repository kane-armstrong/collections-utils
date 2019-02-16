using Microsoft.Azure.ServiceBus;

namespace Armsoft.EventBus.ServiceBus
{
    public interface IServiceBusConnectionManager
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient GetConnection();
    }
}