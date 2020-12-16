using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGameType = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    WinningNumber = table.Column<int>(nullable: true),
                    WinningColor = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_GameType_IdGameType",
                        column: x => x.IdGameType,
                        principalTable: "GameType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGame = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: true),
                    Color = table.Column<string>(maxLength: 10, nullable: true),
                    MoneyBet = table.Column<double>(maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    IsWinner = table.Column<bool>(nullable: false, defaultValue: false),
                    EarnedMoney = table.Column<double>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bet_Game_IdGame",
                        column: x => x.IdGame,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "GameType",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdateDate" },
                values: new object[] { 1, new DateTime(2020, 12, 16, 9, 25, 42, 0, DateTimeKind.Unspecified), "Ruleta", null });

            migrationBuilder.InsertData(
                table: "GameType",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdateDate" },
                values: new object[] { 2, new DateTime(2020, 12, 16, 9, 25, 42, 0, DateTimeKind.Unspecified), "Uno", null });

            migrationBuilder.InsertData(
                table: "GameType",
                columns: new[] { "Id", "CreatedDate", "Name", "UpdateDate" },
                values: new object[] { 3, new DateTime(2020, 12, 16, 9, 25, 42, 0, DateTimeKind.Unspecified), "BlackJack", null });

            migrationBuilder.CreateIndex(
                name: "IX_Bet_IdGame",
                table: "Bet",
                column: "IdGame");

            migrationBuilder.CreateIndex(
                name: "IX_Game_IdGameType",
                table: "Game",
                column: "IdGameType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bet");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "GameType");
        }
    }
}
