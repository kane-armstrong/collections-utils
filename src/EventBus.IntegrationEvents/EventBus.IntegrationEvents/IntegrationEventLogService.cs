using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Armsoft.EventBus.IntegrationEvents
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _db;
        private readonly List<Type> _eventTypes;
        private readonly AsyncLock _mutex = new AsyncLock();

        public IntegrationEventLogService(IntegrationEventLogContext db)
        {
            _db = db;
            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrievePendingEvents()
        {
            return await _db.IntegrationEventLogs
                .Where(e => e.State == EventState.NotPublished)
                .OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeName)))
                .ToListAsync();
        }

        public async Task SaveEvent(IntegrationEvent @event)
        {
            using (await _mutex.LockAsync())
            {
                var eventLogEntry = new IntegrationEventLogEntry(@event);
                _db.IntegrationEventLogs.Add(eventLogEntry);
                await _db.SaveChangesAsync();
            }
        }

        public Task MarkEventAsPublished(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.Published);
        }

        public Task MarkEventAsInProgress(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.InProgress);
        }

        public Task MarkEventAsFailed(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventState.PublishedFailed);
        }

        private async Task UpdateEventStatus(Guid eventId, EventState status)
        {
            using (await _mutex.LockAsync())
            {
                var eventLogEntry = _db.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
                eventLogEntry.State = status;
                if (status == EventState.InProgress)
                {
                    eventLogEntry.TimesSent++;
                }

                _db.IntegrationEventLogs.Update(eventLogEntry);
                await _db.SaveChangesAsync();
            }
        }
    }

}