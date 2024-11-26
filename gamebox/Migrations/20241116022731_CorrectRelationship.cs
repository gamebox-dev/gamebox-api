using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameBox.Migrations
{
    /// <inheritdoc />
    public partial class CorrectRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "User",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Platform",
                newName: "PlatformID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "GamePlatform",
                newName: "GamePlatformID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Game",
                newName: "GameID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CollectionEntry",
                newName: "CollectionEntryID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionEntry_GameID",
                table: "CollectionEntry",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionEntry_PlatformID",
                table: "CollectionEntry",
                column: "PlatformID");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionEntry_Game_GameID",
                table: "CollectionEntry",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "GameID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionEntry_Platform_PlatformID",
                table: "CollectionEntry",
                column: "PlatformID",
                principalTable: "Platform",
                principalColumn: "PlatformID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionEntry_Game_GameID",
                table: "CollectionEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectionEntry_Platform_PlatformID",
                table: "CollectionEntry");

            migrationBuilder.DropIndex(
                name: "IX_CollectionEntry_GameID",
                table: "CollectionEntry");

            migrationBuilder.DropIndex(
                name: "IX_CollectionEntry_PlatformID",
                table: "CollectionEntry");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "User",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "PlatformID",
                table: "Platform",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "GamePlatformID",
                table: "GamePlatform",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "GameID",
                table: "Game",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CollectionEntryID",
                table: "CollectionEntry",
                newName: "ID");
        }
    }
}
