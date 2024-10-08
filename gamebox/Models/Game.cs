namespace GameBox.Models
{
    /// <summary>
    /// Holds information for a game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// List of external IDs referred to by other services
        /// </summary>
        public List<int>? External_ID { get; set; }
        /// <summary>
        /// Title of the game
        /// </summary>
        public string? Title {  get; set; }
        /// <summary>
        /// Summary of the game
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Path to where the image of this game's cover is stored
        /// </summary>
        public string? ImagePath { get; set; }
    }
}
