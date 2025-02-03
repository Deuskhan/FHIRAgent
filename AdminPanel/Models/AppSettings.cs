namespace AdminPanel.Models
{
    public class AppSettings
    {
        public FirebaseSettings Firebase { get; set; } = new();
    }

    public class FirebaseSettings
    {
        public string ProjectId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
} 