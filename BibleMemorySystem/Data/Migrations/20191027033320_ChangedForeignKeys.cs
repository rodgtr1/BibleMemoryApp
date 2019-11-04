using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleMemorySystem.Data.Migrations
{
    public partial class ChangedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Packet_PacketId",
                table: "Slot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slot",
                table: "Slot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packet",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Packet");

            migrationBuilder.AlterColumn<int>(
                name: "PacketId",
                table: "Slot",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "Slot",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PacketId",
                table: "Packet",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slot",
                table: "Slot",
                column: "SlotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packet",
                table: "Packet",
                column: "PacketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Packet_PacketId",
                table: "Slot",
                column: "PacketId",
                principalTable: "Packet",
                principalColumn: "PacketId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Packet_PacketId",
                table: "Slot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Slot",
                table: "Slot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packet",
                table: "Packet");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "PacketId",
                table: "Packet");

            migrationBuilder.AlterColumn<int>(
                name: "PacketId",
                table: "Slot",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Slot",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Packet",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Slot",
                table: "Slot",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packet",
                table: "Packet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Packet_PacketId",
                table: "Slot",
                column: "PacketId",
                principalTable: "Packet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
