using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleMemorySystem.Data.Migrations
{
    public partial class AddedSlotsModelsAndViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slot",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SlotName",
                table: "Packet");

            migrationBuilder.CreateTable(
                name: "Slot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(nullable: false),
                    SlotName = table.Column<string>(nullable: true),
                    Frequency = table.Column<string>(nullable: true),
                    FrequencyDetail = table.Column<string>(nullable: true),
                    FrequencyDay = table.Column<string>(nullable: true),
                    FrequencyMonth = table.Column<string>(nullable: true),
                    PacketId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slot_Packet_PacketId",
                        column: x => x.PacketId,
                        principalTable: "Packet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Slot_PacketId",
                table: "Slot",
                column: "PacketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slot");

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "Packet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SlotName",
                table: "Packet",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
