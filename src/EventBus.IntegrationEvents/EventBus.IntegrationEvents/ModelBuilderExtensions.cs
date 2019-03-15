using Microsoft.EntityFrameworkCore;

namespace Armsoft.EventBus.IntegrationEvents
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureIntegrationEventLogContext(this ModelBuilder @this, IntegrationEventLogContextOptions options)
        {
            @this.Entity<IntegrationEventLogEntry>(entity =>
            {
                var schema = string.IsNullOrEmpty(options.DefaultSchema) ? "dbo" : options.DefaultSchema;
                entity.ToTable("IntegrationEvents", schema);
                entity.HasKey(e => e.EventId);
                entity.Property(e => e.EventId).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreationTime).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.TimesSent).IsRequired();
                entity.Property(e => e.EventTypeFullName).HasMaxLength(400).IsRequired();
            });
        }
    }
}