using Microsoft.EntityFrameworkCore.Migrations;

namespace PLF_Football.Data.Migrations
{
    public partial class ChangePropNameInFixtureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Finished",
                table: "Fixtures",
                newName: "Started");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Started",
                table: "Fixtures",
                newName: "Finished");
        }
    }
}
