using GameBox.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace GameBox.Models.DB
{
    public class GameBoxContext : DbContext
    {
        public DbSet<Game> Game {  get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<GamePlatform> GamePlatform { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CollectionEntry> CollectionEntry { get; set; }

        public string DbPath { get; }

        public GameBoxContext()
        {
            DbPath = EnvironmentUtility.GetVariable("GAMEBOX_SQL_CONNECTION_STRING");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbPath);
        }
    }

    public class Game
    {
        [Key]
        public int GameID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<GamePlatform> GamePlatforms { get; set; }
    }

    public class Platform
    {
        [Key]
        public int PlatformID { get; set; }
        public string Name { get; set; }
        public List<GamePlatform> GamePlatforms { get; set; }
    }

    public class GamePlatform
    {
        [Key]
        public int GamePlatformID { set; get; }
        public int PlatformID { get; set; }
        public Platform Platform { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }
        public List<CollectionEntry> CollectionEntries { get; set; }
    }

    public class CollectionEntry
    {
        [Key]
        public int CollectionEntryID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; }
        public int PlatformID { get; set; }
        public Platform Platform { get; set; }
        public DateTime Added { get; set; }

    }
}
