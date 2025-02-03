using System;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    // Represents a log record from an agent.
    public class LogEntry
    {
        [Key]
        public int LogId { get; set; }
        
        public required string AgentId { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public required string Severity { get; set; }
        
        public required string Message { get; set; }
    }
} 