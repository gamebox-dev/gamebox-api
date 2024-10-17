namespace GameBox.Connectors.IGDB.IGDBResponse
{
    internal class Game
    {
        /// <summary>
        /// ID number of the game as stored in IGDB
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// ID number of the cover as referenced by the Cover collection in IGDB
        /// </summary>
        public int cover { get; set; }
        /// <summary>
        /// The list of platform codes as referenced by the Platform collection in IGDB
        /// </summary>
        public List<int> platforms { get; set; }
        /// <summary>
        /// The name of the game as stored in IGDB
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The description of the game as stored in IGDB
        /// </summary>
        public string summary { get; set; }

        public Game()
        {
            id = 0;
            cover = 0;
            platforms = new List<int>();
            name = string.Empty;
            summary = string.Empty;
        }
    }
}
