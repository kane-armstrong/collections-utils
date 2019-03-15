using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Armsoft.EventBus.IntegrationEvents
{
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry()
        {
        }

        public IntegrationEventLogEntry(IntegrationEvent @event)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeFullName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventState.NotPublished;
            TimesSent = 0;
        }

        public Guid EventId { get; private set; }
        public string EventTypeFullName { get; private set; }

        [NotMapped]
        public string EventTypeName => EventTypeFullName.Split('.')?.Last();

        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }

        public EventState State { get; set; }
        public int TimesSent { get; set; }
        public DateTimeOffset CreationTime { get; private set; }
        public string Content { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;
            return this;
        }
    }
}