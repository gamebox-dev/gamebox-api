namespace GameBox.Models
{
    /// <summary>
    /// Displays the status of the GameBox API server
    /// </summary>
    public class HealthStatus
    {
        /// <summary>
        /// The current status of the server
        /// </summary>
        public string? Status { get; set; }
    }
}
