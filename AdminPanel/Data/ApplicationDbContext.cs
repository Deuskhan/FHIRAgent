using System;
using AdminPanel.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Data
{
    // EF Core database context.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<FhirConfiguration> FhirConfigurations { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
} 