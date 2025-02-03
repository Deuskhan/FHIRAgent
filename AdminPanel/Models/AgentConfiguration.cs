using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AdminPanel.Models
{
    public class AgentConfiguration
    {
        public string AgentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "FHIR Server URL is required")]
        [Display(Name = "FHIR Server URL")]
        public string FhirServerUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Polling Interval is required")]
        [Range(1, 60, ErrorMessage = "Polling interval must be between 1 and 60 minutes")]
        [Display(Name = "Polling Interval (minutes)")]
        public int PollingIntervalMinutes { get; set; } = 5;

        [Display(Name = "Enable Patient Matching")]
        public bool EnablePatientMatching { get; set; } = true;

        [Display(Name = "Enable Data Extraction")]
        public bool EnableDataExtraction { get; set; } = true;

        [Display(Name = "Resource Types to Monitor")]
        public List<string> MonitoredResourceTypes { get; set; } = new()
        {
            "Patient",
            "Observation",
            "Condition",
            "MedicationRequest"
        };

        [Display(Name = "Client ID")]
        public string? ClientId { get; set; }

        [Display(Name = "Client Secret")]
        public string? ClientSecret { get; set; }

        [Display(Name = "Token URL")]
        public string? TokenUrl { get; set; }

        [Display(Name = "Scope")]
        public string? Scope { get; set; }
    }
} 