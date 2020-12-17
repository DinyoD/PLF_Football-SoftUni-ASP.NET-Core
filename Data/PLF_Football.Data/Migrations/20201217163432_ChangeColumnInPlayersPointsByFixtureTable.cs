using Microsoft.EntityFrameworkCore.Migrations;

namespace PLF_Football.Data.Migrations
{
    public partial class ChangeColumnInPlayersPointsByFixtureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersPointsByFixtures_Fixtures_FixtureId",
                table: "PlayersPointsByFixtures");

            migrationBuilder.DropIndex(
                name: "IX_PlayersPointsByFixtures_FixtureId",
                table: "PlayersPointsByFixtures");

            migrationBuilder.RenameColumn(
                name: "FixtureId",
                table: "PlayersPointsByFixtures",
                newName: "Matchday");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Matchday",
                table: "PlayersPointsByFixtures",
                newName: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersPointsByFixtures_FixtureId",
                table: "PlayersPointsByFixtures",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersPointsByFixtures_Fixtures_FixtureId",
                table: "PlayersPointsByFixtures",
                column: "FixtureId",
                principalTable: "Fixtures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
