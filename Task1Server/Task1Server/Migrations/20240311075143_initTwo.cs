using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task1Server.Migrations
{
    public partial class initTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Money = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    Money = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTransactions_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTransactions_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerOneId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlayerTwoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Bet = table.Column<decimal>(type: "numeric", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchHistories_Users_PlayerOneId",
                        column: x => x.PlayerOneId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchHistories_Users_PlayerTwoId",
                        column: x => x.PlayerTwoId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTransactions_ReceiverId",
                table: "GameTransactions",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTransactions_SenderId",
                table: "GameTransactions",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistories_PlayerOneId",
                table: "MatchHistories",
                column: "PlayerOneId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchHistories_PlayerTwoId",
                table: "MatchHistories",
                column: "PlayerTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTransactions");

            migrationBuilder.DropTable(
                name: "MatchHistories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
