using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PLF_Football.Data.Migrations
{
    public partial class AddIdInPlayersUserGamesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PlayersUserGames",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlayersUserGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PlayersUserGames",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PlayersUserGames");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayersUserGames");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PlayersUserGames");
        }
    }
}
