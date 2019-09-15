using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armsoft.EventBus.IntegrationEvents
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrievePendingEvents();

        Task SaveEvent(IntegrationEvent @event);

        Task MarkEventAsPublished(Guid eventId);

        Task MarkEventAsInProgress(Guid eventId);

        Task MarkEventAsFailed(Guid eventId);
    }
}