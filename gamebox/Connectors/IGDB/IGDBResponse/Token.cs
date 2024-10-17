namespace GameBox.Connectors.IGDB.IGDBResponse
{
    internal class Token
    {
        /// <summary>
        /// The authorisation token as received from the IGDB authentication system
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// The time in seconds until the received token expires
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// The authentication scheme for this access token; typically bearer
        /// </summary>
        public string token_type { get; set; }

        public Token()
        {
            access_token = string.Empty;
            expires_in = 0;
            token_type = string.Empty;
        }
    }
}
