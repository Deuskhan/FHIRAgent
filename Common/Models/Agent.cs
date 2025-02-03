using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    // Represents an installed FHIR agent.
    public class Agent
    {
        [Key]
        public int AgentId { get; set; }
        
        [Required]
        public required string MachineName { get; set; }
        
        public string Status { get; set; } = string.Empty;
        
        public DateTime LastSyncTime { get; set; }
        
        public int PatientMatchCount { get; set; }
        
        public int ExtractedDataCount { get; set; }
    }
} 