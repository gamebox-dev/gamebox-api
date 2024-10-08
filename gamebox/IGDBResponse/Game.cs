namespace GameBox.IGDBResponse
{
    internal class Game
    {
        public int? id {  get; set; }
        public int? cover {  get; set; }
        public List<int>? external_games {  get; set; }
        public string? name {  get; set; }
        public string? summary { get; set; }
    }
}
