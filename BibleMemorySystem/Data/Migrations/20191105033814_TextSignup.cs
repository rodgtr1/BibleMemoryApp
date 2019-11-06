using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleMemorySystem.Data.Migrations
{
    public partial class TextSignup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receiver",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPhone = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiver", x => x.Id);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receiver");
        }
    }
}
