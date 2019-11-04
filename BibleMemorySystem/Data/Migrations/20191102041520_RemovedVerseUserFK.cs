using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleMemorySystem.Data.Migrations
{
    public partial class RemovedVerseUserFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verse_AspNetUsers_BibleMemorySystemUserId",
                table: "Verse");

            migrationBuilder.DropIndex(
                name: "IX_Verse_BibleMemorySystemUserId",
                table: "Verse");

            migrationBuilder.DropColumn(
                name: "BibleMemorySystemUserId",
                table: "Verse");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Verse",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Verse");

            migrationBuilder.AddColumn<string>(
                name: "BibleMemorySystemUserId",
                table: "Verse",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Verse_BibleMemorySystemUserId",
                table: "Verse",
                column: "BibleMemorySystemUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Verse_AspNetUsers_BibleMemorySystemUserId",
                table: "Verse",
                column: "BibleMemorySystemUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
