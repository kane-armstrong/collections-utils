using Microsoft.EntityFrameworkCore;
using System;

namespace Armsoft.EventBus.IntegrationEvents
{
    public class IntegrationEventLogContextOptions
    {
        public string DefaultSchema { get; set; }
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }
    }
}