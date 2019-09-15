using System;
using Microsoft.EntityFrameworkCore;

namespace Armsoft.EventBus.IntegrationEvents
{
    public class IntegrationEventLogContext : DbContext
    {
        private readonly IntegrationEventLogContextOptions _contextOptions;

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options, IntegrationEventLogContextOptions contextOptions)
            : base(options)
        {
            _contextOptions = contextOptions ?? throw new ArgumentException(nameof(contextOptions));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureIntegrationEventLogContext(_contextOptions);
            base.OnModelCreating(modelBuilder);
        }
    }
}