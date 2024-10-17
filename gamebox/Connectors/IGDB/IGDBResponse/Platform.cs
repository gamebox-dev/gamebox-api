namespace GameBox.Connectors.IGDB.IGDBResponse
{
    public class Platform
    {
        /// <summary>
        /// ID number of the platform as stored by IGDB
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Common name of the platform, usually abbreviated
        /// </summary>
        public string abbreviation { get; set; }

        public Platform()
        {
            id = 0;
            abbreviation = string.Empty;
        }
    }
}
