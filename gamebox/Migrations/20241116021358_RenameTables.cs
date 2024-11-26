using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBox.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionEntries_Users_UserID",
                table: "CollectionEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatforms_Games_GameID",
                table: "GamePlatforms");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatforms_Platforms_PlatformID",
                table: "GamePlatforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Platforms",
                table: "Platforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlatforms",
                table: "GamePlatforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CollectionEntries",
                table: "CollectionEntries");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Platforms",
                newName: "Platform");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.RenameTable(
                name: "GamePlatforms",
                newName: "GamePlatform");

            migrationBuilder.RenameTable(
                name: "CollectionEntries",
                newName: "CollectionEntry");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatforms_PlatformID",
                table: "GamePlatform",
                newName: "IX_GamePlatform_PlatformID");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatforms_GameID",
                table: "GamePlatform",
                newName: "IX_GamePlatform_GameID");

            migrationBuilder.RenameIndex(
                name: "IX_CollectionEntries_UserID",
                table: "CollectionEntry",
                newName: "IX_CollectionEntry_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platform",
                table: "Platform",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlatform",
                table: "GamePlatform",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CollectionEntry",
                table: "CollectionEntry",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionEntry_User_UserID",
                table: "CollectionEntry",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Game_GameID",
                table: "GamePlatform",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatform_Platform_PlatformID",
                table: "GamePlatform",
                column: "PlatformID",
                principalTable: "Platform",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionEntry_User_UserID",
                table: "CollectionEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Game_GameID",
                table: "GamePlatform");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlatform_Platform_PlatformID",
                table: "GamePlatform");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Platform",
                table: "Platform");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlatform",
                table: "GamePlatform");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CollectionEntry",
                table: "CollectionEntry");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Platform",
                newName: "Platforms");

            migrationBuilder.RenameTable(
                name: "GamePlatform",
                newName: "GamePlatforms");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.RenameTable(
                name: "CollectionEntry",
                newName: "CollectionEntries");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatform_PlatformID",
                table: "GamePlatforms",
                newName: "IX_GamePlatforms_PlatformID");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlatform_GameID",
                table: "GamePlatforms",
                newName: "IX_GamePlatforms_GameID");

            migrationBuilder.RenameIndex(
                name: "IX_CollectionEntry_UserID",
                table: "CollectionEntries",
                newName: "IX_CollectionEntries_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platforms",
                table: "Platforms",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlatforms",
                table: "GamePlatforms",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CollectionEntries",
                table: "CollectionEntries",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionEntries_Users_UserID",
                table: "CollectionEntries",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatforms_Games_GameID",
                table: "GamePlatforms",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlatforms_Platforms_PlatformID",
                table: "GamePlatforms",
                column: "PlatformID",
                principalTable: "Platforms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
