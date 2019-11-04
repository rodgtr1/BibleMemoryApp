using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleMemorySystem.Data.Migrations
{
    public partial class PacketAndSlotToVerse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PacketNumber",
                table: "Verse",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotNumber",
                table: "Verse",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PacketNumber",
                table: "Verse");

            migrationBuilder.DropColumn(
                name: "SlotNumber",
                table: "Verse");
        }
    }
}
