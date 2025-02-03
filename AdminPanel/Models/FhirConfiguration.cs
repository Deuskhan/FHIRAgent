using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models
{
    // Stores SMART on FHIR connection parameters for an agent.
    public class FhirConfiguration
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "FHIR Server URL")]
        public required string FHIRServerUrl { get; set; }
        
        [Display(Name = "Client ID")]
        public string ClientId { get; set; } = string.Empty;
        
        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; } = string.Empty;
        
        [Display(Name = "Redirect URI")]
        public string RedirectUri { get; set; } = string.Empty;
        
        // Additional SMART on FHIR parameters can be added here.
    }
} 