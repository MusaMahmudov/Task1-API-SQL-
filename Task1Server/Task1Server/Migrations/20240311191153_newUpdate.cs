using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1Server.Migrations
{
    public partial class newUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerOneMove",
                table: "MatchHistories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerTwoMove",
                table: "MatchHistories",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerOneMove",
                table: "MatchHistories");

            migrationBuilder.DropColumn(
                name: "PlayerTwoMove",
                table: "MatchHistories");
        }
    }
}
